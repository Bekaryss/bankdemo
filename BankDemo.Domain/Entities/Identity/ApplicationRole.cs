using Microsoft.AspNetCore.Identity;

namespace BankDemo.Domain.Entities.Identity;

public sealed class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName, string description = null)
        : base(roleName)
    {
        Description = description;
        NormalizedName = roleName.ToUpper();
    }

    public string Description { get; set; }
}