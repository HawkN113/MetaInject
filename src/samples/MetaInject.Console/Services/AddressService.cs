using MetaInject.Console.Services.Abstraction;
using Microsoft.Extensions.Logging;
namespace MetaInject.Console.Services;

public class AddressService(ILogger<AddressService> logger): IAddressService
{
    public void Start()
    {
        System.Console.WriteLine("AddressService is doing work!");
        logger.LogInformation("AddressService is doing work!");
    }
}