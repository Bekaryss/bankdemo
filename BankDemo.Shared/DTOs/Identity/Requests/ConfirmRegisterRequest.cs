namespace BankDemo.Shared.DTOs.Identity.Requests;

public class ConfirmRegisterRequest
{
    public string PhoneNumber { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
}