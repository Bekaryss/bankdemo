namespace BankDemo.Infrastructure.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        bool IsAuthenticated();
    }
}
