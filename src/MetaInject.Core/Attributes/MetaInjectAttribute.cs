namespace MetaInject.Core.Attributes;

/// <summary>
/// Indicates that a property or field should be injected with a dependency 
/// when the object is being constructed by a dependency injection container.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MetaInjectAttribute : Attribute;