namespace MetaInject.Validators;

internal static class AppValidator
{
    public static void ThrowIfNull(object? instance, string message)
    {
        if (instance is null)
            throw new InvalidOperationException(message);
    }
    
    public static T ThrowIfNull<T>(T instance) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        return instance;
    }
}