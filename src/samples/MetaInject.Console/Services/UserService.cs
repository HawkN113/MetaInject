using MetaInject.Console.Services.Abstraction;
using MetaInject.Core.Attributes;
namespace MetaInject.Console.Services;

public class UserService: IUserService
{
    [MetaInject]
    public IAddressService AddressService { get; init; }
    
    [MetaInject]
    public required IAddressService1 AddressService1 { get; init; }

    public void DoWork()
    {
        System.Console.WriteLine("UserService is doing work!");
        AddressService.Start();
        AddressService1.Start1();
    }
}