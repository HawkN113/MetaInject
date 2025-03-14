using MetaInject.Core.Attributes;
using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

[MetaValidation]
internal sealed class UserService : IUserService
{
    [MetaInject]
    public IGreetingService GreetingService;

    public string GetCurrent()
    {
        return GreetingService.Greet("Current user");
    }
    public string CurrentInfo() => "User service info";
}