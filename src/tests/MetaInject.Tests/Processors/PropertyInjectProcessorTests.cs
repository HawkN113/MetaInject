using MetaInject.Core.Attributes;
using MetaInject.Processors;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Tests.Processors;

public class PropertyInjectProcessorTests
{
    [Fact]
    public void InjectProperties_InjectsDependencies_WhenServiceIsRegistered()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();

        var processor = new PropertyInjectProcessor(serviceProvider);
        var testInstance = new TestClass();

        // Act
        processor.InjectProperties(testInstance);

        // Assert
        Assert.NotNull(testInstance.MyService);
        Assert.IsType<MyService>(testInstance.MyService);
    }

    [Fact]
    public void InjectProperties_ThrowsException_WhenInstanceIsNull()
    {
        // Arrange
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var processor = new PropertyInjectProcessor(serviceProvider);

        // Act
        var exception = Record.Exception(() => { processor.InjectProperties(null!); });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Contains("The instance to inject properties cannot be null.", exception.Message);
    }

    [Fact]
    public void InjectProperties_ThrowsException_WhenDependencyIsMissing()
    {
        // Arrange
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var processor = new PropertyInjectProcessor(serviceProvider);
        var testInstance = new TestClass();

        // Act
        var exception = Record.Exception(() => { processor.InjectProperties(testInstance); });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void InjectProperties_ThrowsException_WhenPropertyIsReadOnly()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();

        var processor = new PropertyInjectProcessor(serviceProvider);
        var testInstance = new TestClassWithReadOnlyProperty();

        // Act
        var exception = Record.Exception(() => { processor.InjectProperties(testInstance); });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Contains("The property 'MyService' marked with the 'MetaInjectAttribute' should be writable.",
            exception.Message);
    }

    [Fact]
    public void InjectFields_InjectsDependencies_WhenServiceIsRegistered()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();

        var processor = new PropertyInjectProcessor(serviceProvider);
        var testInstance = new TestClassWithFields();

        // Act
        processor.InjectProperties(testInstance);

        // Assert
        Assert.NotNull(testInstance.MyService);
        Assert.IsType<MyService>(testInstance.MyService);
    }

    [Fact]
    public void InjectFields_ThrowsException_WhenDependencyIsMissing()
    {
        // Arrange
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var processor = new PropertyInjectProcessor(serviceProvider);
        var testInstance = new TestClassWithFields();

        // Act
        var exception = Record.Exception(() => { processor.InjectProperties(testInstance); });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenServiceProviderIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
        {
            var propertyInjectProcessor = new PropertyInjectProcessor(null!);
            propertyInjectProcessor.InjectProperties(new TestClass());
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    public interface IMyService;

    public class MyService : IMyService;

    private class TestClass
    {
        [MetaInject] public IMyService? MyService { get; set; }
    }

    private class TestClassWithReadOnlyProperty
    {
        [MetaInject] public IMyService? MyService { get; }
    }

    private class TestClassWithFields
    {
        [MetaInject] public IMyService MyService;
    }
}