using Microsoft.Extensions.DependencyInjection;

namespace BankDemo.Infrastructure.Extensions;

public static class CORSExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        return services.AddCors(cfg =>
        {
            cfg.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });
    }
}