using MetaInject.WebApi.Services.Abstraction;
namespace MetaInject.WebApi.Services;

public class ContractService : IContractService
{
    public string CurrentInfo() => "Contract service info";
}