using MetaInject.Console.Services;
using MetaInject.Console.Services.Abstraction;
using MetaInject.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ILoggerService, LoggerService>();

        var clientNumber = 1;

        services
            .AddTransient<IClientService, ClientService1>(() => clientNumber == 1)
            .AddTransient<IClientService, ClientService2>(() => clientNumber == 2);

        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IAddressService1, AddressService1>();
        services.AddScoped<UserService, UserService>();
    })
    .Build();
try
{
    // Use MetaInject functionality (should be first in the order to register DI correctly)
    host.UseMetaInject();
    
    var userService1 = host.Services
        .GetRequiredService<UserService>()
        .InjectMetaProperties();
    
    var addressService1 = host.Services
        .GetRequiredService<IAddressService1>()
        .InjectMetaProperties();
    
    var clientService = host.Services
        .GetRequiredService<IClientService>()
        .InjectMetaProperties();
    
    clientService.Start();
    userService1.DoWork();
    addressService1.Start1();

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}