using System.Text;
using System.Text.Encodings.Web;
using LoginIdentity.Models;
using LoginIdentity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace LoginIdentity.Controllers;

public class AccountController : Controller
{
    private readonly INotifyService _notifyService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        INotifyService notifyService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _notifyService = notifyService;
    }

    // GET
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber.ToString(),
                Email = model.Email
            }, model.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.ToString());
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var callbackUrl = Url.Action(nameof(ConfirmPhoneNumber), new
                {
                    userId = user.Id, token, phoneNumber = model.PhoneNumber
                });

                await _notifyService.SendSms(model.PhoneNumber,
                    $"Please confirm your phone number by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                await _signInManager.SignInAsync(user, false);
                return LocalRedirect("~/");
            }

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    public async Task<IActionResult> ConfirmPhoneNumber(string userId, string token, string phoneNumber)
    {
        if (userId == null || token == null) Redirect("~/");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber);
        var message = result ? "Thanks for confirming your phone number." : "Error confirming your phone number.";
        return View();
    }
}