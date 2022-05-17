using LoginIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginIdentity.Controllers;
public class AccountController : Controller
{
    // GET
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }
}