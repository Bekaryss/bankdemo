using Microsoft.AspNetCore.Identity;
using System;

namespace BankDemo.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}