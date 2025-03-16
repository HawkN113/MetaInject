using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public class AddressService: IAddressService
{
    public string CurrentInfo() => "Address service info";
}