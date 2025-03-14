using System.Reflection;
using MetaInject.Core.Attributes;
using MetaInject.Validators;
namespace MetaInject.Extensions;

public static class PropertyInjectorExtensions
{
    /// <summary>
    /// Injects dependencies into properties marked with <see cref="MetaInjectAttribute"/>.
    /// </summary>
    /// <param name="instance">The instance whose properties should be injected.</param>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <returns>The instance with injected dependencies.</returns>
    public static T InjectMetaProperties<T>(this T instance)
    {
        if (ServiceProviderAccessor.Provider is null)
            throw new InvalidOperationException(
                "ServiceProvider for 'MetaInject' is not initialized. Please use method '.UseMetaInject()' in startup.");

        var serviceProvider = ServiceProviderAccessor.Provider;

        var properties = instance!.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(MetaInjectAttribute), true));

        foreach (var property in properties)
        {
            var service = serviceProvider?.GetService(property.PropertyType);
            AppValidator.ThrowIfNull(service!,
                $"No service for type '{property.PropertyType.FullName}' is not registered in the service provider.");
            property.SetValue(instance, service);
        }

        return instance;
    }

    /// <summary>
    /// Provides access to the application's <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderAccessor
    {
        /// <summary>
        /// The service provider instance used for dependency resolution.
        /// </summary>
        public static IServiceProvider? Provider { get; set; }
    }
}