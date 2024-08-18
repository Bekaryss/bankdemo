using System.Collections.Generic;
using System.Net;

namespace BankDemo.Infrastructure.Exceptions
{
    public class IdentityException(string message, List<string> errors = default,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) :
        CustomException(message, errors, statusCode)
    {
    }
}
