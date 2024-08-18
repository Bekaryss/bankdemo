using BankDemo.Core.DI;
using BankDemo.Infrastructure.Extensions;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(x =>
        x.SerializerSettings.Converters.Add(new StringEnumConverter()));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAccountServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseInfrastructure(builder.Configuration);
app.MapControllers();

app.Run();
