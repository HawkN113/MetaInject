using System.Reflection;
using MetaInject.Core.Attributes;
using MetaInject.Processors.Abstractions;
using MetaInject.Validators;
using Microsoft.Extensions.DependencyInjection;
namespace MetaInject.Processors;

internal sealed class PropertyInjectProcessor(IServiceProvider rootProvider): IPropertyInjectProcessor
{
    private readonly IServiceProvider _rootProvider = AppValidator.ThrowIfNull(rootProvider);
    
    public void InjectProperties(object instance)
    {
        AppValidator.ThrowIfNull(instance, "The instance to inject properties cannot be null.");

        using var scope = _rootProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        
        InjectProperties(instance, scopedProvider);
        InjectFields(instance, scopedProvider);
    }

    private static void InjectProperties(object instance, IServiceProvider scopedProvider)
    {
        var properties = GetMembersWithMetaInjectAttribute<PropertyInfo>(instance, BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanWrite)
            {
                throw new InvalidOperationException(
                    $"The property '{property.Name}' marked with the '{nameof(MetaInjectAttribute)}' should be writable.");
            }

            var service = scopedProvider.GetService(property.PropertyType);
            ValidateService(service, property.PropertyType);
            property.SetValue(instance, service);
        }
    }

    private static void InjectFields(object instance, IServiceProvider scopedProvider)
    {
        var fields = GetMembersWithMetaInjectAttribute<FieldInfo>(instance, BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var service = scopedProvider.GetService(field.FieldType);
            ValidateService(service, field.FieldType);
            field.SetValue(instance, service);
        }
    }

    private static IEnumerable<TMember> GetMembersWithMetaInjectAttribute<TMember>(object instance, BindingFlags bindingFlags)
        where TMember : MemberInfo
    {
        return instance.GetType()
            .GetMembers(bindingFlags)
            .OfType<TMember>()
            .Where(m => m.IsDefined(typeof(MetaInjectAttribute), true));
    }

    private static void ValidateService(object? service, Type serviceType)
    {
        AppValidator.ThrowIfNull(service, 
            $"No service of type '{serviceType.FullName}' is registered in the service provider.");
    }
}