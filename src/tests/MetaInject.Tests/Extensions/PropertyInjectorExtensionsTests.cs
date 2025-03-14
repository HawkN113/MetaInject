using MetaInject.Core.Attributes;
using MetaInject.Extensions;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Tests.Extensions;

public class PropertyInjectorExtensionsTests
{
    [Fact]
    public void InjectMetaProperties_InjectsDependencies_WhenServiceIsRegistered()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();
        
        PropertyInjectorExtensions.ServiceProviderAccessor.Provider = serviceProvider;

        var testInstance = new TestClass();

        // Act
        testInstance.InjectMetaProperties();

        // Assert
        Assert.NotNull(testInstance.MyService);
        Assert.IsType<MyService>(testInstance.MyService);
    }

    [Fact]
    public void InjectMetaProperties_ThrowsException_WhenServiceProviderIsNull()
    {
        // Arrange
        PropertyInjectorExtensions.ServiceProviderAccessor.Provider = null;
        var testInstance = new TestClass();

        // Act
        var exception = Record.Exception(() =>
        {
            testInstance.InjectMetaProperties();
        });
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Contains("ServiceProvider for 'MetaInject' is not initialized. ", exception.Message);
    }

    [Fact]
    public void InjectMetaProperties_ThrowsException_WhenDependencyIsMissing()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();
        PropertyInjectorExtensions.ServiceProviderAccessor.Provider = serviceProvider;

        var testInstance = new TestClass();

        // Act
        var exception = Record.Exception(() =>
        {
            testInstance.InjectMetaProperties();
        });
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void InjectMetaProperties_SkipsPropertiesWithoutAttribute()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();
        
        PropertyInjectorExtensions.ServiceProviderAccessor.Provider = serviceProvider;

        var testInstance = new TestClassWithoutInjectAttribute();

        // Act
        testInstance.InjectMetaProperties();

        // Assert
        Assert.Null(testInstance.MyService);
    }

    // Helper Classes
    public interface IMyService;

    public class MyService : IMyService;

    public class TestClass
    {
        [MetaInject]
        public IMyService? MyService { get; set; }
    }

    public class TestClassWithoutInjectAttribute
    {
        public IMyService? MyService { get; set; } 
    }
}