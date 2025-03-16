using MetaInject.Console.Services.Abstraction;
using MetaInject.Core.Attributes;
namespace MetaInject.Console.Services;

public class ClientService : IClientService
{
    [MetaInject] public virtual required ILoggerService LoggerService { get; init; }

    public void Start()
    {
        LoggerService.LogInfo("ClientService1 is doing work!");
    }
}