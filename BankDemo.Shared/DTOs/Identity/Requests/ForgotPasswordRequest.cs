using System.ComponentModel.DataAnnotations;

namespace BankDemo.Shared.DTOs.Identity.Requests;

public class ForgotPasswordRequest
{
    [Required] public string PhoneNumber { get; set; }
}