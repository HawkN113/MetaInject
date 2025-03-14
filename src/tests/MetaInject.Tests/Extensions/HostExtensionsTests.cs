using MetaInject.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
namespace MetaInject.Tests.Extensions;

public class HostExtensionsTests
{
    [Fact]
    public void UseMetaInject_ShouldThrowException_WhenHostIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
        {
            HostExtensions.UseMetaInject(null!);
        });
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Contains("Host instance cannot be null", exception.Message);
    }

    [Fact]
    public void UseMetaInject_ShouldSetServiceProvider_WhenHostIsValid()
    {
        // Arrange
        var services = new ServiceCollection();
        var hostMock = new Mock<IHost>();
        hostMock.Setup(h => h.Services).Returns(services.BuildServiceProvider());

        // Act
        var result = hostMock.Object.UseMetaInject();

        // Assert
        Assert.NotNull(PropertyInjectorExtensions.ServiceProviderAccessor.Provider);
        Assert.Equal(hostMock.Object.Services, PropertyInjectorExtensions.ServiceProviderAccessor.Provider);
        Assert.Equal(hostMock.Object, result);
    }
}