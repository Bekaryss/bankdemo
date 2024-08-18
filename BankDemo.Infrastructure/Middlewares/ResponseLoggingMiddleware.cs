using BankDemo.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace BankDemo.Infrastructure.Middlewares;

public class ResponseLoggingMiddleware(ICurrentUserService currentUser) : IMiddleware
{
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        await next(httpContext);
        var originalBody = httpContext.Response.Body;
        using var newBody = new MemoryStream();
        httpContext.Response.Body = newBody;
        string responseBody;
        if (httpContext.Request.Path.ToString().Contains("tokens"))
        {
            responseBody = "[Redacted] Contains Sensitive Information.";
        }
        else if (httpContext.Request.Path.ToString().Contains("jobs"))
        {
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
            return;
        }
        else
        {
            newBody.Seek(0, SeekOrigin.Begin);
            responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        }

        string userId = _currentUser.GetUserId().ToString();
        LogContext.PushProperty("UserId", userId);
        LogContext.PushProperty("StatusCode", httpContext.Response.StatusCode);
        LogContext.PushProperty("ResponseTimeUTC", DateTime.UtcNow);
        Log.ForContext("ResponseHeaders",
                httpContext.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
            .ForContext("ResponseBody", responseBody)
            .Information(
                "HTTP {RequestMethod} Request to {RequestPath} by {RequesterUserId} has Status Code {StatusCode}.",
                httpContext.Request.Method, httpContext.Request.Path,
                string.IsNullOrEmpty(userId) ? "Anonymous" : userId, httpContext.Response.StatusCode);
        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originalBody);
    }
}