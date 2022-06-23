namespace LoginIdentity.Models
{
    public class ResetPasswordModelView
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
