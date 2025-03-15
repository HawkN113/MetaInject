namespace MetaInject.Core.Attributes;

/// <summary>
/// Indicates that a property should be injected with a dependency 
/// when the object is being constructed by a dependency injection container.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MetaInjectAttribute : Attribute;