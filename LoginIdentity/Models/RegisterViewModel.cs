using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LoginIdentity.Models;

public class RegisterViewModel
{
    [DisplayName("First Name")] public string FirstName { get; set; }

    [DisplayName("LastName")] public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    [DisplayName("Email")]
    public string Email { get; set; }

    [DataType(DataType.PhoneNumber)]
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; }

    [DisplayName("DateOfBirth")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset DateOfBirth { get; set; }

    [DisplayName("Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DisplayName("ConfirmPassword")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}