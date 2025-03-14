using MetaInject.Console.Services.Abstraction;
namespace MetaInject.Console.Services;

public class AddressService1: IAddressService1
{
    public void Start1()
    {
        System.Console.WriteLine("AddressService1 is doing work!");
    }
}