using System;
using System.Collections.Generic;
using System.Net;

namespace BankDemo.Infrastructure.Exceptions
{
    public class CustomException(string message, List<string> errors = default,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : Exception(message)
    {
        public List<string> ErrorMessages { get; } = errors;
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}
