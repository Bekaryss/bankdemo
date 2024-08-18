using BankDemo.Domain.Entities.Identity;
using BankDemo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankDemo.Infrastructure.Persistence;

public abstract class BaseDbContext(DbContextOptions options) :
    IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim,
    IdentityUserToken<string>>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyIdentityConfiguration();
    }
}