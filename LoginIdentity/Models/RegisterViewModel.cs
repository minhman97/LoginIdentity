using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LoginIdentity.Models;

public class RegisterViewModel
{
    [DisplayName("First Name")]
    [Required(ErrorMessage = "Please input First Name")]
    public string FirstName { get; set; }

    [DisplayName("Last Name")]
    [Required(ErrorMessage = "Please input Last Name")]
    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^([\w\.\-]+)@([\w]+)((\.(\w){2,3})+)$", ErrorMessage = "Email invalid")]
    [Required(ErrorMessage = "Please input Email")]
    [DisplayName("Email")]
    public string Email { get; set; }

    [DataType(DataType.PhoneNumber)]
    [Required(ErrorMessage = "Please input Phone Number")]
    //[RegularExpression(@"^(\+\d{1,3}|\d{3})?(\d{3})(\d{3})(\d{4})", ErrorMessage = "Phone number invalid")]
    [DisplayName("Phone Number")]
    public int PhoneNumber { get; set; }

    [DisplayName("Date of birth")]
    [Required(ErrorMessage = "Please input Date of birth")]
    public DateTimeOffset DateOfBirth { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = "Please input Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DisplayName("Confirm Password")]
    [Required(ErrorMessage = "Please input Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}