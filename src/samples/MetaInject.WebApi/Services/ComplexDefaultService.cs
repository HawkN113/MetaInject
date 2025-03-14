using System.Text;
using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public class ComplexDefaultService(
    IUserService userService,
    IAddressService addressService,
    INotesService notesService,
    IContractService contractService) : IComplexService
{
    public string GetCurrentInfo()
    {
        var builder = new StringBuilder("Implementation of complex service using default constructor");
        builder.AppendLine(userService.CurrentInfo());
        builder.AppendLine(addressService.CurrentInfo());
        builder.AppendLine(notesService.CurrentInfo());
        builder.AppendLine(contractService.CurrentInfo());
        return builder.ToString();
    }
}