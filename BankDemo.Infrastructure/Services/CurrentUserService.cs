using BankDemo.Infrastructure.Extensions;
using BankDemo.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankDemo.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor = accessor;

        public string Name => _accessor.HttpContext?.User.Identity?.Name;

        public Guid GetUserId()
        {
            return IsAuthenticated()
                ? Guid.Parse(_accessor.HttpContext?.User.GetUserId() ?? Guid.Empty.ToString())
                : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext?.User.GetUserEmail() : string.Empty;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
