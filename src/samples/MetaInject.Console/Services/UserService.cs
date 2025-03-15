using MetaInject.Console.Services.Abstraction;
using MetaInject.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace MetaInject.Console.Services;

public class UserService: IUserService
{
    [MetaInject] public virtual required ILoggerService LoggerService { get; init; }
    [MetaInject] public virtual ILogger<UserService> Logger { get; set; }

    public async Task<string> GetCurrentUserAsync()
    {
        var userAccount = "1234567890";
        LoggerService.LogInfo($"Account: {userAccount}");
        return await Task.FromResult(userAccount);
    }
}