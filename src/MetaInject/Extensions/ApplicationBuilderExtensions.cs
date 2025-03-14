using MetaInject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Validation all DI using MetaInject
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseMetaValidation(this IApplicationBuilder app)
    {
        var validationList = app.ApplicationServices.GetService<HashSet<ServiceValidation>>();
        var serviceList = app.ApplicationServices.GetService<HashSet<ServiceDescription>>();
        if (validationList is null || serviceList is null)
            throw new InvalidOperationException(
                "Use method '.AddMetaInject()' in startup before the method '.UseMetaValidation()'.");

        var registeredServiceTypes = new HashSet<string>(serviceList.Select(x => x.ServiceType),
            StringComparer.CurrentCultureIgnoreCase);
        if (validationList.Count == 0) return app;
        foreach (var serviceValidation in validationList)
        {
            ValidateServices(serviceValidation.PropertyTypes, registeredServiceTypes);
            ValidateServices(serviceValidation.FieldTypes, registeredServiceTypes);
        }

        return app;
    }

    private static void ValidateServices(IEnumerable<ItemServiceType> serviceTypes,
        HashSet<string> registeredServiceTypes)
    {
        foreach (var service in serviceTypes)
        {
            if (!registeredServiceTypes.Contains(service.ItemType))
            {
                throw new ArgumentNullException(service.ItemName, $"Service {service.ItemType} is not registered.");
            }
        }
    }
}