using Contracts;
using Microsoft.Extensions.DependencyInjection;
using ModuleTwo;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace ModuleOne;

public static class Configurer
{
    public static ServiceProvider Configure(InMemNetwork inMemNetwork)
    {
        var services = new ServiceCollection();

        services.AddRebus(rebusConfigurer =>
            {
                return rebusConfigurer
                    .Transport(t => t.UseInMemoryTransport(inMemNetwork, $"{Constants.ModuleOneBus}-queue"))
                    .Options(Constants.CommonBusOptionsConfigurer)
                    .Serialization(Constants.CommonBusSerializerConfig)
                    .Routing(config => config
                        .TypeBased()
                        .MapAssemblyDerivedFrom<IQuery2>($"{Constants.ModuleTwoBus}-queue")
                        .MapAssemblyDerivedFrom<ICommand2>($"{Constants.ModuleTwoBus}-queue")
                        .MapAssemblyDerivedFrom<IResponse1>($"{Constants.HostBus}-queue"));
            },
            key: Constants.ModuleOneBus);

        services.AutoRegisterHandlersFromAssemblyOf<Command1Handler>();

        var ioc = services.BuildServiceProvider();

        ioc.StartRebus();

        return ioc;
    }
}