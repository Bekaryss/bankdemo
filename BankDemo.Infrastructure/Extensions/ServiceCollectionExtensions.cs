using BankDemo.Infrastructure.Services;
using BankDemo.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankDemo.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static T GetOptions<T>(
            this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);

            return options;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization();
            services.AddIdentity(configuration);
            services.AddHealthChecks();
            services.AddCorsPolicy();
            services.AddApplicationDatabase();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMiddlewares(configuration);
            services.AddSwaggerDocumentation();

            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
