using Microsoft.AspNetCore.Identity;

namespace BankDemo.Domain.Entities.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string Description { get; set; }
}