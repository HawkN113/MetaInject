using MetaInject.Validators;
using Microsoft.Extensions.Hosting;
namespace MetaInject.Extensions;

public static class HostExtensions
{
    /// <summary>
    /// Configures the host to enable MetaInject dependency injection.
    /// </summary>
    /// <param name="host">The application host.</param>
    /// <returns>The configured <see cref="IHost"/> instance.</returns>
    public static IHost UseMetaInject(this IHost host)
    {
        AppValidator.ThrowIfNull(host, "Host instance cannot be null. Ensure the application is properly initialized.");
        PropertyInjectorExtensions.ServiceProviderAccessor.Provider = host.Services;
        return host;
    }
}