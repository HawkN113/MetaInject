using System.Text;
using MetaInject.Core.Attributes;
using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

[MetaValidation]
public class ComplexPropertiesService : IComplexPropertiesService
{
    [MetaInject] public required IUserService UserService { get; init; }
    [MetaInject] public required IAddressService AddressService { get; init; }
    [MetaInject] public required INotesService NotesService { get; init; }
    [MetaInject] public required IContractService ContractService { get; init; }

    public string GetCurrentInfo()
    {
        var builder = new StringBuilder("Implementation of complex service using properties");
        builder.AppendLine(UserService.CurrentInfo());
        builder.AppendLine(AddressService.CurrentInfo());
        builder.AppendLine(NotesService.CurrentInfo());
        builder.AppendLine(ContractService.CurrentInfo());
        return builder.ToString();
    }
}