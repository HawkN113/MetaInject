using MetaInject.Core.Attributes;
using MetaInject.Extensions;
using MetaInject.Models;
using MetaInject.Processors.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public interface ITestService;

    public class TestService : ITestService;

    public class OtherTestService : ITestService;

    private readonly IServiceCollection _services = new ServiceCollection();

    [MetaValidation]
    public class MyValidatableService : IMyValidatableService
    {
        [MetaInject] public ITestService? InjectedService { get; set; }
    }
    
    public interface IMyValidatableService { }
    
    
    [Fact]
    public void AddMetaInject_RegistersRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMetaInject();

        // Assert
        var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<HashSet<ServiceDescription>>());
        Assert.NotNull(provider.GetService<HashSet<ServiceValidation>>());
        Assert.NotNull(provider.GetService<IPropertyInjectProcessor>());
    }

    [Fact]
    public void AddMetaInject_AddsServicesToServiceList()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestService>();

        // Act
        services.AddMetaInject();
        var provider = services.BuildServiceProvider();
        var serviceList = provider.GetRequiredService<HashSet<ServiceDescription>>();

        // Assert
        Assert.Contains(serviceList, s => s.ServiceType == typeof(ITestService).FullName);
    }

    [Fact]
    public void AddMetaInject_RegistersValidationForMetaValidationAttribute()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<IMyValidatableService, MyValidatableService>();

        // Act
        services.AddMetaInject();
        var provider = services.BuildServiceProvider();
        var validationList = provider.GetRequiredService<HashSet<ServiceValidation>>();

        // Assert
        Assert.Contains(validationList, v => v.ServiceType == typeof(IMyValidatableService).FullName);
    }
    
    [Fact]
    public void AddMetaInject_RegistersValidationAndServiceLists()
    {
        // Arrange
        _services.AddScoped<ITestService, TestService>();
        _services.AddMetaInject();
        var provider = _services.BuildServiceProvider();

        // Act
        var serviceList = provider.GetService<HashSet<ServiceDescription>>();
        var validationList = provider.GetService<HashSet<ServiceValidation>>();

        // Assert
        Assert.NotNull(serviceList);
        Assert.NotEmpty(serviceList);
        Assert.Contains(serviceList, s => s.ServiceType == typeof(ITestService).FullName);

        Assert.NotNull(validationList);
    }
    
    [Fact]
    public void AddScoped_ShouldRegisterService_WhenConditionIsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = true;

        // Act
        services.AddScoped<ITestService, TestService>(() => condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<TestService>(service);
    }

    [Fact]
    public void AddScoped_ShouldNotRegisterService_WhenConditionIsFalse()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = false;

        // Act
        services.AddScoped<ITestService, TestService>(() => condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.Null(service);
    }

    [Fact]
    public void AddSingleton_ShouldRegisterService_WhenConditionIsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = true;

        // Act
        services.AddSingleton<ITestService, TestService>(() => condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<TestService>(service);
    }

    [Fact]
    public void AddSingleton_ShouldNotRegisterService_WhenConditionIsFalse()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = false;

        // Act
        services.AddSingleton<ITestService, TestService>(() => condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.Null(service);
    }

    [Fact]
    public void AddTransient_ShouldRegisterService_WhenConditionIsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = true;

        // Act
        services.AddTransient<ITestService, TestService>(() => condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<TestService>(service);
    }

    [Fact]
    public void AddTransient_ShouldNotRegisterService_WhenConditionIsFalse()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = false;

        // Act
        services.AddTransient<ITestService, TestService>(() => condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.Null(service);
    }

    [Fact]
    public void AddScoped_ShouldNotRegisterDifferentImplementation_WhenConditionIsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        var condition = true;

        // Act
        services.AddScoped<ITestService, TestService>(() => condition);
        services.AddScoped<ITestService, OtherTestService>(() => !condition);

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<ITestService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<TestService>(service);
    }
}