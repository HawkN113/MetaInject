using MetaInject.Console.Services;
using MetaInject.Console.Services.Abstraction;
using MetaInject.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
try
{
    var userService = host.Services
        .GetRequiredService<IUserService>();
    
    var addressService = host.Services
        .GetRequiredService<IAddressService>();
    
    var clientService = host.Services
        .GetRequiredService<IClientService>();
    
    var logService = host.Services
        .GetRequiredService<ILoggerService>();
    
    clientService.Start();
    var account = await userService.GetCurrentUserAsync();
    var address = addressService.GetAddressByAccount(account);
    logService.LogInfo($"Account: {account}, Address: {address}");

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}