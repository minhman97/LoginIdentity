using System.ComponentModel.DataAnnotations;

namespace LoginIdentity.Data;

public class Account
{
    [Key]
    public int Id { get; set; }
    [MaxLength(150)]
    public string FirstName { get; set; }
    [MaxLength(150)]
    public string LastName { get; set; }
}