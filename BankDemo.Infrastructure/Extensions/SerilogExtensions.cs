using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.Refit.Destructurers;

namespace BankDemo.Infrastructure.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder RegisterSerilog(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpLogging(cfg =>
                {
                    cfg.LoggingFields = HttpLoggingFields.All;
                });
            });

            builder.UseSerilog((_, _, serilogConfig) =>
            {
                serilogConfig
                    .Enrich.FromLogContext()
                    .Enrich.WithSpan()
                    .Enrich.WithExceptionDetails(
                        new DestructuringOptionsBuilder()
                            .WithDefaultDestructurers()
                            .WithDestructurers(
                            [
                                new DbUpdateExceptionDestructurer(),
                                new ApiExceptionDestructurer()
                            ]))
                    .Enrich.WithDemystifiedStackTraces()
                    .Enrich.WithProcessId()
                    .Enrich.WithThreadId()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                    .WriteTo.Console();
            });

            return builder;
        }
    }
}
