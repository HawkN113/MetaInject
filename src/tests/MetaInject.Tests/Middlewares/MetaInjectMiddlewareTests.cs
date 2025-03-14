using MetaInject.Middlewares;
using MetaInject.Processors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
namespace MetaInject.Tests.Middlewares;

public class MetaInjectMiddlewareTests
{
    private readonly RequestDelegate _next = Mock.Of<RequestDelegate>();

    [Fact]
    public async Task MetaInjectMiddleware_InjectsDependencies_WhenFromServicesAttributeExists()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        var endpoint = new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(typeof(TestController).GetMethod(nameof(TestController.TestMethod))!),
            "TestEndpoint");

        httpContext.SetEndpoint(endpoint);

        var middleware = new MetaInjectMiddleware(_next, new PropertyInjectProcessor(serviceProvider));

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(httpContext.Items.ContainsKey("service"));
        Assert.IsType<MyService>(httpContext.Items["service"]);
    }

    [Fact]
    public async Task MetaInjectMiddleware_SkipsInjection_WhenNoFromServicesAttribute()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMyService, MyService>()
            .BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        var endpoint = new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(
                typeof(TestController).GetMethod(nameof(TestController.MethodWithoutInjection))!),
            "TestEndpoint");

        httpContext.SetEndpoint(endpoint);

        var middleware = new MetaInjectMiddleware(_next, new PropertyInjectProcessor(serviceProvider));

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.False(httpContext.Items.ContainsKey("service"));
    }

    [Fact]
    public async Task MetaInjectMiddleware_HandlesNullEndpointGracefully()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();
        
        var httpContext = new DefaultHttpContext();
        var middleware = new MetaInjectMiddleware(_next, new PropertyInjectProcessor(serviceProvider));

        // Act & Assert (should not throw)

        var result = await Record.ExceptionAsync(async () => await middleware.InvokeAsync(httpContext));
        Assert.Null(result);

    }

    private interface IMyService;

    public class MyService : IMyService;

    private class TestController
    {
        public void TestMethod([FromServices] IMyService service)
        {
            // Do something with the service
        }

        public void MethodWithoutInjection(IMyService service)
        {
            // Do something with the service
        }
    }
}