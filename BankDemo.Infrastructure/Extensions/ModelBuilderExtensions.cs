using BankDemo.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankDemo.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyIdentityConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity => { entity.ToTable("Users", "Identity"); });
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("Roles", "Identity");
                entity.Metadata.RemoveIndex(new[] { entity.Property(r => r.NormalizedName).Metadata });
                entity.HasIndex(r => new { r.NormalizedName }).HasDatabaseName("RoleNameIndex").IsUnique();
            });
            builder.Entity<ApplicationRoleClaim>(entity => { entity.ToTable("RoleClaims", "Identity"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles", "Identity"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims", "Identity"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins", "Identity"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens", "Identity"); });
        }
    }
}
