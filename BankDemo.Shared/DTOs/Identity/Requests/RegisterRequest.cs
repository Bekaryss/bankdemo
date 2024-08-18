using System.ComponentModel.DataAnnotations;

namespace BankDemo.Shared.DTOs.Identity.Requests;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string PasswordConfirmation { get; set; }
}