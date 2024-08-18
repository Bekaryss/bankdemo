namespace BankDemo.Shared.DTOs.Identity.Requests;

public class ResetPasswordRequest
{
    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public string Token { get; set; }
}