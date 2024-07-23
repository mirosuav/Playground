using Contracts;
using Microsoft.Extensions.DependencyInjection;
using ModuleOne;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace Runner;

internal class Program
{
    public static ServiceProvider HostIoc = null!;
    public static InMemNetwork HostInMemNetwork = null!;
    public static ServiceProvider ModuleOneIoC = null!;
    public static ServiceProvider ModuleTwoIoC = null!;
    public static int RequestsCount = 5;


    static async Task Main(string[] args)
    {
        ConfigureAllModules();

        await ConsoleFlowWaitForResponse.Run(HostIoc.GetRequiredService<IBus>());
        //await ConsoleFlowAsynchronous.Run(HostIoc.GetRequiredService<IBus>());

        Console.ReadLine();
        await ModuleTwoIoC.DisposeAsync();
        await ModuleOneIoC.DisposeAsync();
        await HostIoc.DisposeAsync();
    }
    
    public static void ConfigureAllModules()
    {
        HostInMemNetwork = new InMemNetwork();
        ModuleOneIoC = ModuleTwo.Configurer.Configure(HostInMemNetwork);
        ModuleTwoIoC = ModuleOne.Configurer.Configure(HostInMemNetwork);
        HostIoc = Runner.Configurer.Configure(HostInMemNetwork);
    }
}

