using MetaInject.Console.Services.Abstraction;
using MetaInject.Core.Attributes;
namespace MetaInject.Console.Services;

public class ClientService2 : IClientService
{
    [MetaInject]
    public required ILoggerService LoggerService { get; init; }
    public void Start()
    {
        LoggerService.LogInfo("ClientService2 is doing work!");
    }
}