namespace BankDemo.Shared.DTOs.Identity.Responses;

public class ConfirmResponse
{
    public ConfirmResponse(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}