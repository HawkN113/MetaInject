namespace MetaInject.Models;

internal record ServiceDescription(string ServiceType, Microsoft.Extensions.DependencyInjection.ServiceLifetime Lifetime);