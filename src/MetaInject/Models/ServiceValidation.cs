namespace MetaInject.Models;

internal record ServiceValidation(string ServiceType, ItemServiceType[] PropertyTypes, ItemServiceType[] FieldTypes);

internal record ItemServiceType(string ItemName, string ItemType);