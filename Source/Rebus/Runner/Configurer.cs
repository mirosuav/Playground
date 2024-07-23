using Contracts;
using Microsoft.Extensions.DependencyInjection;
using ModuleOne;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace Runner;
internal static class Configurer
{
    public static ServiceProvider Configure(InMemNetwork inMemNetwork)
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
                        .MapAssemblyDerivedFrom<IQuery1>($"{Constants.ModuleOneBus}-queue")
                        .MapAssemblyDerivedFrom<ICommand1>($"{Constants.ModuleOneBus}-queue"));
            },
            key: Constants.HostBus);

        services.AutoRegisterHandlersFromAssemblyOf<Response1Handler>();

        var ioc = services.BuildServiceProvider();

        ioc.StartRebus();

        return ioc;
    }
}

