using Microsoft.AspNetCore.Authentication;

namespace LoginIdentity.Models;

public class LoginViewModel
{
    private IList<AuthenticationScheme> _externalLogins;
    public string ReturnUrl { get; set; }
    public LoginViewModel()
    {
    }

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;

    public IList<AuthenticationScheme> ExternalLogins
    {
        get => _externalLogins;
        set => _externalLogins = value;
    }
}