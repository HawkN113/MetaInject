using System.Reflection;
using MetaInject.Core.Attributes;
using MetaInject.Models;
using MetaInject.Processors;
using MetaInject.Processors.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Conditionally registers a scoped service of type <typeparamref name="TService"/> 
    /// with an implementation of <typeparamref name="TImplementation"/> in the dependency injection container.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="condition">If true, the service is registered; otherwise, it is skipped.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services,
        Func<bool> condition)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(condition);
        if (condition())
            services.AddScoped<TService, TImplementation>();
        return services;
    }

    /// <summary>
    /// Conditionally registers a singleton service of type <typeparamref name="TService"/> 
    /// with an implementation of <typeparamref name="TImplementation"/> in the dependency injection container.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="condition">A function that returns true if the service should be registered; otherwise, false.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services,
        Func<bool> condition)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(condition);
        if (condition())
            services.AddSingleton<TService, TImplementation>();
        return services;
    }

    /// <summary>
    /// Conditionally registers a transient service of type <typeparamref name="TService"/> 
    /// with an implementation of <typeparamref name="TImplementation"/> in the dependency injection container.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="condition">A function that returns true if the service should be registered; otherwise, false.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services,
        Func<bool> condition)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(condition);
        if (condition())
            services.AddTransient<TService, TImplementation>();
        return services;
    }

    /// <summary>
    /// Add Meta inject
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMetaInject(this IServiceCollection services)
    {
        var serviceList = new HashSet<ServiceDescription>();
        var validationList = new HashSet<ServiceValidation>();

        foreach (var service in services.ToArray())
        {
            serviceList.Add(new ServiceDescription(service.ServiceType.FullName!, service.Lifetime));

            if (service.ImplementationType == null)
                continue;

            var isValidatable = service.ImplementationType.IsDefined(typeof(MetaValidationAttribute), inherit: false);

            if (!isValidatable)
                continue;

            var propertyTypes = service.ImplementationType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(MetaInjectAttribute)))
                .Select(p => new ItemServiceType(p.Name, p.PropertyType.FullName!))
                .ToArray();

            var fieldTypes = service.ImplementationType
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.IsDefined(typeof(MetaInjectAttribute)))
                .Select(f => new ItemServiceType(f.Name, f.FieldType.FullName!))
                .ToArray();

            validationList.Add(new ServiceValidation(service.ServiceType.FullName!, propertyTypes, fieldTypes));
        }

        services.AddSingleton(serviceList);
        services.AddSingleton(validationList);
        services.AddTransient<IPropertyInjectProcessor, PropertyInjectProcessor>();

        return services;
    }
}