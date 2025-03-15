using System.Reflection;
using MetaInject.Core.Attributes;
using MetaInject.Interceptors;
using Castle.DynamicProxy;
using MetaInject.Interceptors.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers dependencies with support for the <see cref="MetaInjectAttribute"/>.
    /// Automatically creates proxy objects for classes with virtual properties marked with <see cref="MetaInjectAttribute"/>.
    /// </summary>
    /// <param name="services">The DI container's service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMetaInject(this IServiceCollection services)
    {
        services.AddSingleton<IMetaInjectInterceptor, MetaInjectInterceptor>();
        services.AddSingleton<IProxyGenerator, ProxyGenerator>();

        foreach (var service in services.ToArray())
        {
            if (service.ImplementationType == null)
                continue;

            var isInjectable = service.ImplementationType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(prop => prop.IsDefined(typeof(MetaInjectAttribute), true) &&
                             prop is
                             {
                                 CanRead: true, CanWrite: true, GetMethod.IsVirtual: true, SetMethod.IsVirtual: true
                             });

            if (!isInjectable)
                continue;

            services.Remove(service);
            services.Add(service.Lifetime, service.ServiceType,
                provider => CreateProxy(provider, service.ImplementationType));
        }

        return services;
    }

    private static object CreateProxy(IServiceProvider provider, Type implementationType)
    {
        var implementation = ActivatorUtilities.CreateInstance(provider, implementationType);
        var interceptor = provider.GetRequiredService<IMetaInjectInterceptor>();
        var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
        return proxyGenerator.CreateClassProxyWithTarget(implementationType, implementation, interceptor);
    }

    private static void Add(this IServiceCollection services, ServiceLifetime lifetime, Type serviceType,
        Func<IServiceProvider, object> factory)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Transient:
                services.AddTransient(serviceType, factory);
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(serviceType, factory);
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton(serviceType, factory);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }
    }
}