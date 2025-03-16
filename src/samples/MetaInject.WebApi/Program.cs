using MetaInject.Extensions;
using MetaInject.WebApi.Services;
using MetaInject.WebApi.Services.Abstraction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGreetingService, GreetingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IComplexService, ComplexDefaultService>();
builder.Services.AddTransient<IComplexPropertiesService, ComplexPropertiesService>();

// Register MetaInject for advanced DI functionality (should be last in the order)
builder.Services.AddMetaInject();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/GetCurrentUser", (IUserService userService) =>
    {
        return Results.Ok(userService.GetCurrent());
    })
    .WithTags("User service")
    .WithDescription(
        "Retrieves the current user using the [UserService] implementation. The service includes a [GreetingService] field, which is injected via the [MetaInject] attribute.")
    .WithOpenApi();

app.MapGet("api/app0/GetCurrentInfo", (IComplexService service) =>
    {
        return Results.Ok(service.GetCurrentInfo());
    })
    .WithTags("Default implementation")
    .WithDescription(
        "Retrieves information using the default [ComplexService] implementation, which is configured with constructor parameters for dependency injection.")
    .WithOpenApi();

app.MapGet("api/app2/GetCurrentInfo", (IComplexPropertiesService service) =>
    {
        return Results.Ok(service.GetCurrentInfo());
    })
    .WithMetadata()
    .WithTags("Properties implementation")
    .WithDescription(
        "Retrieves information using the [ComplexPropertiesService] implementation. The service uses 'virtual' properties injected with the [MetaInject] attribute.")
    .WithOpenApi();

await app.RunAsync();

