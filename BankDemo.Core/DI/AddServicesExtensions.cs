using BankDemo.Core.Interfaces;
using BankDemo.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankDemo.Core.DI
{
    public static class AddServicesExtensions
    {
        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            services.AddTransient<IDefaultAccountService, DefaultAccountService>();
            services.AddTransient<IDepositAccountService, DepositAccountService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IAccountManagerService, AccountManagerService>();
            return services;
        }
    }
}
