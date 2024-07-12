using BenchmarkDotNet.Running;
using Contracts;
using Microsoft.Extensions.DependencyInjection;
using ModuleOne;
using Rebus;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Rebus.Workers.TplBased;

namespace Runner;

internal class Program
{
    public static ServiceProvider HostIoc = null!;
    public static InMemNetwork HostInMemNetwork = null!;

    static Task Main(string[] args)
    {
        //await ConsoleFlow();
        BenchmarkRunner.Run<RebusBenchmarks>();
        Console.ReadLine();
        return Task.FromResult(true);
    }

    private static async Task ConsoleFlow()
    {
        Console.WriteLine($">>>>> Starting {nameof(ConsoleFlow)}...");

        ConfigureAllModules();

        var bus = HostIoc.GetRequiredService<IBus>();

        //var requestTasks = Enumerable.Range(0, 10)
        //    .Select(i => bus.SendRequest<int>(new ModuleOneRequest(i, 10)))
        //    .ToList();

        //var results = await Task.WhenAll(requestTasks);
        //Console.WriteLine($">>>>> Host received {results.Length} responses...");

        var response = await bus.SendRequest<int>(new ModuleOneRequest(5, 7));
        Console.WriteLine($">>>>> Host received response: {response}");
    }


    public static void ConfigureAllModules()
    {
        HostInMemNetwork = new InMemNetwork();
        _ = ModuleTwo.Configurer.Configure(HostInMemNetwork);
        _ = ModuleOne.Configurer.Configure(HostInMemNetwork);
        HostIoc = Configure(HostInMemNetwork);
    }

    private static ServiceProvider Configure(InMemNetwork inMemNetwork)
    {
        var services = new ServiceCollection();

        services.AddRebus(rebusConfigurer =>
            {
                return rebusConfigurer
                    .Transport(t => t.UseInMemoryTransport(inMemNetwork, $"{Constants.HostBus}-queue"))
                    .Options(Constants.CommonBusOptionsConfigurer)
                    .Serialization(Constants.CommonBusSerializerConfig)
                    .Routing(config => config
                        .TypeBased()
                        .MapAssemblyDerivedFrom<IModuleOneRequest>($"{Constants.ModuleOneBus}-queue")
                        .MapAssemblyDerivedFrom<IModuleOneSimpleRequest>($"{Constants.ModuleOneBus}-queue"));
            },
            key: Constants.HostBus);

        var ioc = services.BuildServiceProvider();

        ioc.StartRebus();

        return ioc;
    }
}

