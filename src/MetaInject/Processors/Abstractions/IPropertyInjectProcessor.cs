namespace MetaInject.Processors.Abstractions;

public interface IPropertyInjectProcessor
{
    void InjectProperties(object instance);
}