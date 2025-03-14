using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public sealed class GreetingService: IGreetingService
{
    public string Greet(string name)
    {
        return $"Hello, {name}!";
    }
}