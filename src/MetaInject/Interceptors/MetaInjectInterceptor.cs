using System.Reflection;
using Castle.DynamicProxy;
using MetaInject.Core.Attributes;
using MetaInject.Interceptors.Abstractions;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Interceptors;

internal class MetaInjectInterceptor(IServiceProvider rootProvider) : IMetaInjectInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        if (invocation.InvocationTarget is null)
        {
            invocation.Proceed();
            return;
        }

        var targetType = invocation.InvocationTarget.GetType();
        var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.IsDefined(typeof(MetaInjectAttribute), inherit: true) &&
                           prop is
                           {
                               CanRead: true, CanWrite: true, GetMethod.IsVirtual: true, SetMethod.IsVirtual: true
                           })
            .ToArray();

        if (properties.Length > 0)
        {
            using var scope = rootProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            foreach (var property in properties)
            {
                var dependency = scopedProvider.GetService(property.PropertyType);
                if (dependency is not null)
                {
                    property.SetValue(invocation.InvocationTarget, dependency);
                }
            }
        }

        invocation.Proceed();
    }
}