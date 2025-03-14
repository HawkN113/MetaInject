using System.Reflection;
using MetaInject.Processors.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace MetaInject.Middlewares;

/// <summary>
/// Middleware for injecting dependencies into properties marked with MetaInject attributes.
/// This middleware processes action method parameters marked with <see cref="FromServicesAttribute"/> 
/// and ensures their dependencies are properly injected before execution.
/// </summary>
/// <param name="next">The next middleware in the pipeline.</param>
public class MetaInjectMiddleware(
    RequestDelegate next,
    IPropertyInjectProcessor propertyInjectProcessor)
{
    private readonly IPropertyInjectProcessor _propertyInjectProcessor =
        propertyInjectProcessor ?? throw new ArgumentNullException(nameof(propertyInjectProcessor));

    public async Task InvokeAsync(HttpContext context)
    {
        var methodInfo = context.GetEndpoint()?.Metadata.GetMetadata<MethodInfo>();
        if (methodInfo is not null)
        {
            foreach (var parameter in methodInfo.GetParameters()
                         .Where(p => p.IsDefined(typeof(FromServicesAttribute))))
            {
                if (context.RequestServices.GetService(parameter.ParameterType) is not { } service) continue;
                _propertyInjectProcessor.InjectProperties(service);
                context.Items[parameter.Name!] = service;
            }
        }

        await next(context);
    }
}