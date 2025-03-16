using MetaInject.Core.Attributes;
using MetaInject.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using IInvocation = Castle.DynamicProxy.IInvocation;
namespace MetaInject.Tests.Interceptors;

public class MetaInjectInterceptorTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly Mock<IInvocation> _invocationMock;
    private readonly MetaInjectInterceptor _interceptor;

    public MetaInjectInterceptorTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IMyDependency, MyDependency>();
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _invocationMock = new Mock<IInvocation>();
        _interceptor = new MetaInjectInterceptor(_serviceProvider);
    }

    [Fact]
    public void Intercept_ShouldInjectDependency_WhenPropertyIsVirtual()
    {
        // Arrange
        var target = new MyClass();
        _invocationMock.SetupGet(i => i.InvocationTarget).Returns(target);

        // Act
        _interceptor.Intercept(_invocationMock.Object);

        // Assert
        Assert.NotNull(target.MyDependency);
        Assert.IsType<MyDependency>(target.MyDependency);
    }

    [Fact]
    public void Intercept_ShouldProceed_WhenNoInjectableProperties()
    {
        // Arrange
        var target = new MyClassWithoutInjectableProperties();
        _invocationMock.SetupGet(i => i.InvocationTarget).Returns(target);

        // Act
        _interceptor.Intercept(_invocationMock.Object);

        // Assert
        _invocationMock.Verify(i => i.Proceed(), Times.Once);
    }

    [Fact]
    public void Intercept_ShouldNotInject_WhenDependencyNotRegistered()
    {
        // Arrange
        var target = new MyClassWithUnregisteredDependency();
        _invocationMock.SetupGet(i => i.InvocationTarget).Returns(target);

        // Act
        _interceptor.Intercept(_invocationMock.Object);

        // Assert
        Assert.Null(target.UnregisteredDependency);
    }

    private interface IMyDependency { }
    private class MyDependency : IMyDependency { }

    private class MyClass
    {
        [MetaInject]
        public virtual IMyDependency MyDependency { get; set; }
    }

    private class MyClassWithoutInjectableProperties
    {
        public IMyDependency MyDependency { get; set; }
    }

    private class MyClassWithUnregisteredDependency
    {
        [MetaInject]
        public virtual IUnregisteredDependency UnregisteredDependency { get; set; }
    }

    private interface IUnregisteredDependency { }
}