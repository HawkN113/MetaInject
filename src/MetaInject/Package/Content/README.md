# MetaInject 

MetaInject is a simple and powerful Dependency Injection (DI) tool for .NET. With the `[MetaInject]` attribute, you can inject dependencies directly into properties or fields without needing a constructor. MetaInject works seamlessly with ASP.NET Core and other .NET projects, simplifying dependency management.

## Features
- **[MetaInject] attribute**: Enables property injection, allowing dependencies to be injected into properties instead of using constructors.
- **Seamless Web API Integration**: Fully compatible with ASP.NET Core and other .NET-based projects.

---

## Getting Started

### Prerequisites

- .NET 8 or higher.

---

## Usage

Use the following namespaces:

```csharp
using MetaInject.Core.Attributes;
using MetaInject.Extensions;
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

With MetaInject, you can remove the constructor and inject dependencies directly into properties. The property must:
- be **virtual**
- have both a **getter** and a **setter**

```csharp
public class ComplexService : IComplexService
{
    [MetaInject] public virtual required IUserService UserService { get; init; }
    [MetaInject] public virtual required IAddressService AddressService { get; init; }
    [MetaInject] public virtual required INotesService NotesService { get; init; }
    [MetaInject] public virtual required IContractService ContractService { get; init; }
}
```
or

```csharp
public class ComplexService : IComplexService
{
    [MetaInject] public virtual IUserService UserService { get; set; }
    [MetaInject] public virtual IAddressService AddressService { get; set; }
    [MetaInject] public virtual INotesService NotesService { get; set; }
    [MetaInject] public virtual IContractService ContractService { get; set; }
}
```
---

## Use in Minimal API .NET 8

Use `.AddMetaInject()` for advanced DI functionality:

```csharp
// Registering sample services with the DI container
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
Use the service in API:

```csharp
app.MapGet("api/app2/GetCurrentInfo", (IComplexPropertiesService service) =>
    {
        return Results.Ok(service.GetCurrentInfo());
    })
    .WithMetadata()
    .WithTags("Properties implementation")
    .WithDescription(
        "Retrieves information using the [ComplexPropertiesService] implementation. The service uses 'virtual' properties injected with the [MetaInject] attribute.")
    .WithOpenApi();
```

---

## Use in console application .NET 8

Use `.AddMetaInject()` for advanced DI functionality:

```csharp
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Registering sample services with the DI container
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IClientService, ClientService>();
        
        // Register MetaInject for advanced DI functionality (should be last in the order)
        services.AddMetaInject();
    })
    .Build();
```

Use the service in console application:

```csharp
    var userService = host.Services
        .GetRequiredService<IUserService>();
```