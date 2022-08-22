#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoginAndRegis.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage = "must be at least 2 chars")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage = "must be at least 2 chars")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(8, ErrorMessage = "must be at least 8 chars")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [NotMapped]
    [Compare("Password", ErrorMessage = "doesn't match password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public string FullName()
    {
        return FirstName + " " + LastName;
    }
}