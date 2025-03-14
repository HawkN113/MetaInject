using MetaInject.Extensions;
using MetaInject.Middlewares;
using MetaInject.WebApi.Services;
using MetaInject.WebApi.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Use MetaInject functionality (should be first in the order to register DI correctly)
app.UseMetaInject();
// Enable class validation during the DI process
app.UseMetaValidation();
// MetaInject middleware to resolve dependencies marked with [FromServices] attribute in requests
app.UseMiddleware<MetaInjectMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/GetCurrentUser", ([FromServices] IUserService userService) =>
    {
        return Results.Ok(userService.GetCurrent());
    })
    .WithTags("User service")
    .WithDescription(
        "Retrieves the current user using the [UserService] implementation. The service includes a [GreetingService] field, which is injected via the [MetaInject] attribute.")
    .WithOpenApi();

app.MapGet("api/app0/GetCurrentInfo", ([FromServices] IComplexService service) =>
    {
        return Results.Ok(service.GetCurrentInfo());
    })
    .WithTags("Default implementation")
    .WithDescription(
        "Retrieves information using the default [ComplexService] implementation, which is configured with constructor parameters for dependency injection.")
    .WithOpenApi();

app.MapGet("api/app1/GetCurrentInfo", ([FromServices] IComplexFieldsService service) =>
    {
        return Results.Ok(service.GetCurrentInfo());
    })
    .WithTags("Fields implementation")
    .WithDescription(
        "Retrieves information using the [ComplexFieldsService] implementation. The service uses fields injected with the [MetaInject] attribute.")
    .WithOpenApi();

app.MapGet("api/app2/GetCurrentInfo", ([FromServices] IComplexPropertiesService service) =>
    {
        return Results.Ok(service.GetCurrentInfo());
    })
    .WithTags("Properties implementation")
    .WithDescription(
        "Retrieves information using the [ComplexPropertiesService] implementation. The service uses properties injected with the [MetaInject] attribute.")
    .WithOpenApi();

await app.RunAsync();

