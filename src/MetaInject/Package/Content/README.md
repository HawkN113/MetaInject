# MetaInject 

MetaInject is a simple and powerful Dependency Injection (DI) tool for .NET. With the `[MetaInject]` attribute, you can inject dependencies directly into properties or fields without needing a constructor. The `[MetaValidation]` attribute ensures that your DI setup is correct before the application starts. MetaInject works seamlessly with ASP.NET Core and other .NET projects, simplifying dependency management. It supports `.InjectMetaProperties()` extension to resolve DI inside the service. It also supports conditional service registration via `.AddTransient`, `.AddSingleton`, and `.AddScoped`.

## Features
- **[MetaInject] attribute**: Enables property injection, allowing dependencies to be injected into properties instead of using constructors.
- **[MetaValidation] attribute**: Ensures all dependencies are properly injected during application startup.
- **.InjectMetaProperties() extension**: Allows DI resolution inside the service.
- **Conditional service registration**: Supports dynamic conditions for `.AddTransient`, `.AddSingleton`, and `.AddScoped` services.
- **Seamless Web API Integration**: Fully compatible with ASP.NET Core and other .NET-based projects.

---

## Getting Started

### Prerequisites

- .NET 8 or higher.

---

## Usage

Use the following namespaces:

```csharp
using MetaInject.Extensions;
using MetaInject.Middlewares;
```

### Scenario: Injecting dependencies without a constructor

When you have a service with a large number of parameters, such as a service constructor with over 7 parameters, using a constructor can become cumbersome. MetaInject simplifies this by allowing you to inject dependencies directly into properties.

#### Before MetaInject (constructor injection)

```csharp
public class ComplexService : IComplexService
{
    private readonly IUserService _userService;
    private readonly IAddressService _addressService;
    private readonly INotesService _notesService;
    private readonly IContractService _contractService;

    public ComplexService(
        IUserService userService,
        IAddressService addressService,
        INotesService notesService,
        IContractService contractService)
    {
        _userService = userService;
        _addressService = addressService;
        _notesService = notesService;
        _contractService = contractService;
    }
}
```

#### After MetaInject (property injection)

With MetaInject, you can remove the constructor and inject dependencies directly into properties.

```csharp
public class ComplexService : IComplexService
{
    [MetaInject] public required IUserService UserService { get; init; }
    [MetaInject] public required IAddressService AddressService { get; init; }
    [MetaInject] public required INotesService NotesService { get; init; }
    [MetaInject] public required IContractService ContractService { get; init; }
}
```

### Using [MetaValidation] for dependency validation

If you want to ensure that the dependencies are correctly injected during startup, you can use the `[MetaValidation]` attribute. This attribute validates that all properties marked with `[MetaInject]` are correctly populated.

```csharp
[MetaValidation]
public class ComplexService : IComplexService
{
    [MetaInject] public required IUserService UserService { get; init; }
    [MetaInject] public required IAddressService AddressService { get; init; }
    [MetaInject] public required INotesService NotesService { get; init; }
    [MetaInject] public required IContractService ContractService { get; init; }
}
```

### Property Injection with fields

MetaInject also supports injecting dependencies into readonly fields. This approach can be useful when you need to ensure immutability.

```csharp
public class ComplexService : IComplexService
{
    [MetaInject] public readonly IUserService UserService;
    [MetaInject] public readonly IAddressService AddressService;
    [MetaInject] public readonly INotesService NotesService;
    [MetaInject] public readonly IContractService ContractService;
}
```

---

## Use in Minimal API .NET 8

Use `.AddMetaInject()` for advanced DI functionality:

```csharp
// Registering services with the DI container
builder.Services.AddSingleton<IGreetingService, GreetingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IComplexService, ComplexDefaultService>();
builder.Services.AddScoped<IComplexFieldsService, ComplexFieldsService>();
builder.Services.AddScoped<IComplexPropertiesService, ComplexPropertiesService>();

// Register MetaInject for advanced DI functionality (should be last in the order)
builder.Services.AddMetaInject();
```

Use `.UseMetaInject()` to enable DI functionality.

Use `.UseMetaValidation()` to enable class validation during the DI process.

Use `app.UseMiddleware<MetaInjectMiddleware>()` to resolve dependencies marked with the `[FromServices]` attribute in requests.

```csharp
var app = builder.Build();

// Use MetaInject DI functionality (should be first in the order to register DI correctly)
app.UseMetaInject();

// Enable class validation during the DI process
app.UseMetaValidation();

// MetaInject middleware to resolve dependencies marked with [FromServices] attribute in requests
app.UseMiddleware<MetaInjectMiddleware>();
```

---

## Conditional service registration in MetaInject

MetaInject allows for conditional service registration in your Dependency Injection container. You can register services such as Transient, Singleton, or Scoped based on dynamic conditions.

### Conditional Service registration methods

#### AddScoped with condition

```csharp
services.AddScoped<IUserService, UserService>(() => DateTime.Now.DayOfWeek == DayOfWeek.Monday);
```

#### AddSingleton with condition

```csharp
services.AddSingleton<IAppSettings, AppSettings>(() => Environment.GetEnvironmentVariable("APP_MODE") == "Production");
```

#### AddTransient with condition

```csharp
services.AddTransient<ILogger, ConsoleLogger>(() => Debugger.IsAttached);
```
