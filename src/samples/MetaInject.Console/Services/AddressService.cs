using MetaInject.Console.Services.Abstraction;
using MetaInject.Core.Attributes;
namespace MetaInject.Console.Services;

public class AddressService: IAddressService
{
    [MetaInject]
    public virtual required ILoggerService LoggerService { get; init; }

    public string GetAddressByAccount(string account)
    {
        var address = "123 Main St";
        LoggerService.LogInfo($"Address '{address}' for the account '{account}'");
        return address;
    }
}