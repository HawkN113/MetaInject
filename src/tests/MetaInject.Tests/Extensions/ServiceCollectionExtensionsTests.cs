using Castle.DynamicProxy;
using MetaInject.Core.Attributes;
using MetaInject.Extensions;
using MetaInject.Interceptors.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Tests.Extensions;


public class ServiceCollectionExtensionsTests()
{
    private IServiceCollection _services;
    private ServiceProvider _provider;

    [Fact]
    public void AddMetaInject_Should_RegisterInterceptorAndProxyGenerator()
    {
        // Arrange
        _services = new ServiceCollection();
        _services.AddTransient<IDependency, Dependency>();
        _services.AddTransient<IInjectableService, InjectableService>();
        _services.AddMetaInject();
        _provider = _services.BuildServiceProvider();
        
        // Act
        var interceptor = _provider.GetService<IMetaInjectInterceptor>();
        var proxyGenerator = _provider.GetService<IProxyGenerator>();

        // Assert
        Assert.NotNull(interceptor);
        Assert.NotNull(proxyGenerator);
    }

    [Fact]
    public void AddMetaInject_Should_CreateProxyForTransientService()
    {
        // Arrange
        _services = new ServiceCollection();
        _services.AddTransient<IDependency, Dependency>();
        _services.AddTransient<IInjectableService, InjectableService>();
        _services.AddMetaInject();
        _provider = _services.BuildServiceProvider();
        
        // Act
        var service = _provider.GetService<IInjectableService>();
        
        // Assert
        Assert.NotNull(service);
        Assert.NotNull(service.Dependency);
        Assert.IsType<Dependency>(service.Dependency);
    }
    
    [Fact]
    public void AddMetaInject_Should_CreateProxyForScopedService()
    {
        // Arrange
        _services = new ServiceCollection();
        _services.AddScoped<IDependency, Dependency>();
        _services.AddScoped<IInjectableService, InjectableService>();
        _services.AddMetaInject();
        _provider = _services.BuildServiceProvider();
        
        // Act
        var service = _provider.GetService<IInjectableService>();
        
        // Assert
        Assert.NotNull(service);
        Assert.NotNull(service.Dependency);
        Assert.IsType<Dependency>(service.Dependency);
    }
    
    [Fact]
    public void AddMetaInject_Should_CreateProxyForSingletonService()
    {
        // Arrange
        _services = new ServiceCollection();
        _services.AddSingleton<IDependency, Dependency>();
        _services.AddSingleton<IInjectableService, InjectableService>();
        _services.AddMetaInject();
        _provider = _services.BuildServiceProvider();
        
        // Act
        var service = _provider.GetService<IInjectableService>();
        
        // Assert
        Assert.NotNull(service);
        Assert.NotNull(service.Dependency);
        Assert.IsType<Dependency>(service.Dependency);
    }

    [Fact]
    public void AddMetaInject_Should_NotModify_NonInjectableService()
    {
        // Arrange
        _services = new ServiceCollection();
        _services.AddTransient<INonInjectableService, NonInjectableService>();
        using var provider = _services.BuildServiceProvider();

        // Act
        var service = provider.GetService<INonInjectableService>();
        Assert.NotNull(service);
        Assert.Null(service.Dependency);
    }
}

public interface IDependency {}
public class Dependency : IDependency {}

public interface IInjectableService
{
    IDependency Dependency { get; set; }
}

public class InjectableService : IInjectableService
{
    [MetaInject]
    public virtual IDependency Dependency { get; set; } = null!;
}

public interface INonInjectableService
{
    IDependency? Dependency { get; }
}

public class NonInjectableService : INonInjectableService
{
    public IDependency? Dependency { get; }
}