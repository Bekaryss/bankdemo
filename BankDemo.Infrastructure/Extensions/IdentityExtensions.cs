using BankDemo.Domain.Entities.Identity;
using BankDemo.Infrastructure.Exceptions;
using BankDemo.Infrastructure.Persistence;
using BankDemo.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BankDemo.Infrastructure.Extensions;

public static class IdentityExtensions
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddJwtAuthentication();
        return services;
    }

    internal static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services)
    {
        var jwtSettings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
        services.AddSingleton<JwtSettings>(jwtSettings);

        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };
                bearer.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                            throw new IdentityException("Authentication error.",
                                statusCode: HttpStatusCode.Unauthorized);

                        return Task.CompletedTask;
                    },

                    OnForbidden = _ => throw new IdentityException(
                        "You do not have access to this resource.",
                        statusCode: HttpStatusCode.Forbidden),

                    OnAuthenticationFailed = _ =>
                        throw new IdentityException("Authentication error.", statusCode: HttpStatusCode.Unauthorized)
                };
            });

        return services;
    }
}