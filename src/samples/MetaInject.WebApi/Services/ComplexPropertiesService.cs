using System.Text;
using MetaInject.Core.Attributes;
using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public class ComplexPropertiesService : IComplexPropertiesService
{
    [MetaInject] public virtual IUserService UserService { get; set; }
    [MetaInject] public virtual IAddressService AddressService { get; set; }
    [MetaInject] public virtual INotesService NotesService { get; set; }
    [MetaInject] public virtual IContractService ContractService { get; set; }

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