using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BankDemo.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.None);
        });
        return app;
    }

    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (!assembly.IsDynamic)
                {
                    string xmlFile = $"{assembly.GetName().Name}.xml";
                    string xmlPath = Path.Combine(baseDirectory, xmlFile);
                    if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);
                }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}