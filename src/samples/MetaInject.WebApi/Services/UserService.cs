using MetaInject.Core.Attributes;
using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public class UserService : IUserService
{
    [MetaInject] public virtual IGreetingService GreetingService { get; set; }

    public string GetCurrent()
    {
        return GreetingService.Greet("Current user");
    }

    public string CurrentInfo() => "User service info";
}