using MetaInject.Console.Services.Abstraction;
using MetaInject.Core.Attributes;
using Microsoft.Extensions.Logging;
namespace MetaInject.Console.Services;

public class UserService: IUserService
{
    [MetaInject] public virtual required ILoggerService LoggerService { get; init; }
    [MetaInject] public virtual required ILogger<UserService> Logger { get; init; }

    public async Task<string> GetCurrentUserAsync()
    {
        var userAccount = "1234567890";
        LoggerService.LogInfo($"Using custom logger service for account: {userAccount}");
        Logger.LogInformation("Using ILogger for account: {UserAccount}", userAccount);
        return await Task.FromResult(userAccount);
    }
}