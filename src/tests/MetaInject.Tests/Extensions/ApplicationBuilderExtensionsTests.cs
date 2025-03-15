using MetaInject.Extensions;
using MetaInject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
namespace MetaInject.Tests.Extensions;

public class ApplicationBuilderExtensionsTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IApplicationBuilder> _appBuilderMock;

    public ApplicationBuilderExtensionsTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _appBuilderMock = new Mock<IApplicationBuilder>();

        _appBuilderMock
            .Setup(x => x.ApplicationServices)
            .Returns(_serviceProviderMock.Object);
    }

    [Fact]
    public void UseMetaValidation_ThrowsInvalidOperationException_WhenNoMetaInjectConfigured()
    {
        // Arrange
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(HashSet<ServiceValidation>)))
            .Returns(null!);
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(HashSet<ServiceDescription>)))
            .Returns(null!);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _appBuilderMock.Object.UseMetaValidation());
        Assert.Contains("Use method '.AddMetaInject()'", ex.Message);
    }

    [Fact]
    public void UseMetaValidation_ThrowsArgumentNullException_WhenServiceNotRegistered()
    {
        // Arrange
        var validationSet = new HashSet<ServiceValidation>
        {
            new("IMyService", [new("IMyService", "MyServiceProperty")], [new("IMyService", "MyServiceProperty")])
        };

        _serviceProviderMock.Setup(sp => sp.GetService(typeof(HashSet<ServiceValidation>)))
            .Returns(validationSet);
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(HashSet<ServiceDescription>)))
            .Returns(new HashSet<ServiceDescription>());

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _appBuilderMock.Object.UseMetaValidation());
        Assert.Equal("IMyService", ex.ParamName);
        Assert.Contains("No service for type 'MyServiceProperty' is registered in the service provider.", ex.Message);
    }

    [Fact]
    public void UseMetaValidation_DoesNotThrow_WhenAllServicesAreRegistered()
    {
        // Arrange
        var validationSet = new HashSet<ServiceValidation>
        {
            new("IMyService", [new("IMyService2", "MyServiceProperty")], [new("IMyService2", "MyServiceProperty")])
        };

        var serviceDescriptions = new HashSet<ServiceDescription>
        {
            new("IMyService", ServiceLifetime.Transient),
            new("MyServiceProperty", ServiceLifetime.Transient)
        };
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(HashSet<ServiceValidation>)))
            .Returns(validationSet);
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(HashSet<ServiceDescription>)))
            .Returns(serviceDescriptions);

        // Act & Assert
        var exception = Record.Exception(() => _appBuilderMock.Object.UseMetaValidation());
        Assert.Null(exception);
    }
}