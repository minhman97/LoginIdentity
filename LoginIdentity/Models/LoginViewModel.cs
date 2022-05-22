using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace LoginIdentity.Models;

public class LoginViewModel
{
    public LoginViewModel()
    {
        ExternalLogins = new List<AuthenticationScheme>();
    }

    public string ReturnUrl { get; set; }
    [Required(ErrorMessage = "Please input Username.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Please input Password.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;

    public IList<AuthenticationScheme> ExternalLogins { get; set; }
}