using MetaInject.Console.Services.Abstraction;
namespace MetaInject.Console.Services;

public class LoggerService : ILoggerService
{
    public void LogInfo(string message)
    {
        System.Console.Out.WriteLine(message);
    }
}