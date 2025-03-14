using System.Text;
using MetaInject.Core.Attributes;
using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

[MetaValidation]
internal class ComplexFieldsService : IComplexFieldsService
{
    [MetaInject] public required IUserService UserService;
    [MetaInject] public required IAddressService AddressService;
    [MetaInject] public readonly INotesService NotesService;
    [MetaInject] public readonly IContractService ContractService;

    public string GetCurrentInfo()
    {
        var builder = new StringBuilder("Implementation of complex service using fields");
        builder.AppendLine(UserService.CurrentInfo());
        builder.AppendLine(AddressService.CurrentInfo());
        builder.AppendLine(NotesService.CurrentInfo());
        builder.AppendLine(ContractService.CurrentInfo());
        return builder.ToString();
    }
}