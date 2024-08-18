using BankDemo.Infrastructure.Exceptions;
using BankDemo.Infrastructure.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace BankDemo.Infrastructure.Middlewares;

internal class ExceptionMiddleware(
    ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            if (exception is not CustomException && exception.InnerException != null)
                while (exception.InnerException != null)
                    exception = exception.InnerException;

            _logger.LogError(exception, exception.Message);
            var responseModel = await ErrorResult<string>.ReturnErrorAsync(exception.Message);
            responseModel.Source = exception.Source;
            responseModel.Exception = exception.Message;

            switch (exception)
            {
                case CustomException e:
                    response.StatusCode = responseModel.ErrorCode = (int)e.StatusCode;
                    responseModel.Messages = e.ErrorMessages;
                    break;
                default:
                    response.StatusCode = responseModel.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.Exception = "InternalServerError";
                    responseModel.Messages = new List<string>();
                    break;
            }

            string result = JsonConvert.SerializeObject(responseModel, Formatting.Indented);
            await response.WriteAsync(result);
        }
    }
}