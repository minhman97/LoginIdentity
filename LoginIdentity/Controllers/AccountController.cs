using LoginIdentity.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginIdentity.Controllers;

public class AccountController : Controller
{
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
        }

        return View(model);
    }
}