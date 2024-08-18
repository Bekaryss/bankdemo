using BankDemo.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankDemo.Infrastructure.Extensions;

public static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration config)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ResponseLoggingMiddleware>();

        return app;
    }

    internal static IServiceCollection AddMiddlewares(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ExceptionMiddleware>();
        services.AddSingleton<RequestLoggingMiddleware>();
        services.AddSingleton<ResponseLoggingMiddleware>();

        return services;
    }
}