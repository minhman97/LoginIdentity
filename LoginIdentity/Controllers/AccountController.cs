using System.Text;
using LoginIdentity.Models;
using LoginIdentity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace LoginIdentity.Controllers;

public class AccountController : Controller
{
    private readonly INotifyService _notifyService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IEmailSender _emailSender;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        INotifyService notifyService, IEmailService emailService, IEmailSender emailSender)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _notifyService = notifyService;
        _emailService = emailService;
        _emailSender = emailSender;
    }

    // GET
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded) return LocalRedirect("~/");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return LocalRedirect("~/");
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
            var userExisted = await _userManager.FindByEmailAsync(model.Email);
            if (userExisted != null)
            {
                ModelState.AddModelError("", "Email existed");
                return View(model);
            }

            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber.ToString(),
                Email = model.Email
            }, model.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "Confirm your account",
                    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                

                //var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.ToString());
                //token = WebEntokenrs.Base64UrlEntoken(Encoding.UTF8.GetBytes(token));

                //var callbackUrl = Url.Action(nameof(ConfirmPhoneNumber), new
                //{
                //    userId = user.Id,
                //    token,
                //    phoneNumber = model.PhoneNumber
                //});

                // await _notifyService.SendSms(model.PhoneNumber,
                //     $"Please confirm your phone number by <a href='{HtmlEntokenr.Default.Entoken(callbackUrl)}'>clicking here</a>.");


                await _signInManager.SignInAsync(user, false);
                return LocalRedirect("~/");
            }

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordViewModel());
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Your email doesn't exist in system");
                return View(model);
            }
            //if (!(await _userManager.IsEmailConfirmedAsync(user)))
            //{
            // Don't reveal that the user does not exist or is not confirmed
            //    return View();
            //}

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token }, protocol: Request.Scheme);
            await _emailService.SendEmailAsync(user.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            return RedirectToAction("Confirmation", "Account",
                new ConfirmationViewModel() { Title = "Forgot password", StatusMessage = "Please check your email to reset your password." });
        }
        // If we got this far, something failed, redisplay form
        return View();
    }

    public IActionResult Confirmation(ConfirmationViewModel model)
    {
        if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.StatusMessage))
            return View(new ConfirmationViewModel()
            {
                Title = "Error",
                StatusMessage = "Something wrong"
            });
        return View(model);
    }

    public IActionResult ResetPassword(string email, string token)
    {
        return View(new ResetPasswordModelView()
        {
            Email = email,
            Token = token,
        });
    }
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordModelView model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        if(result.Succeeded)
            return RedirectToAction("Confirmation", "Account",
                new ConfirmationViewModel() { Title = "Reset password", StatusMessage = "Password changed succesfully" });
        return RedirectToAction("Confirmation", "Account",
                new ConfirmationViewModel() { Title = "Reset password", StatusMessage = result.Errors.First().Description });
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