using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddServiceHandlers();
    })
    .Build();

try
{
    //var appHandler = host.Services.GetRequiredService<IAppHandler>();
    //await appHandler.HandleAsync(args);
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}