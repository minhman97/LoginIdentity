using System.ComponentModel.DataAnnotations;

namespace LoginIdentity.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please input Email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
