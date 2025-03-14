namespace MetaInject.Core.Attributes;

/// <summary>
/// Marks a class for validation by the dependency injection container 
/// during the application's startup or initialization phase.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MetaValidationAttribute : Attribute;