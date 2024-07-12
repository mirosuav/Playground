using Contracts;
using Microsoft.Extensions.DependencyInjection;
using ModuleTwo;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Rebus.Workers.TplBased;

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
                        .MapAssemblyDerivedFrom<IModuleTwoRequest>($"{Constants.ModuleTwoBus}-queue"));
            },
            key: Constants.ModuleOneBus);

        services.AutoRegisterHandlersFromAssemblyOf<ModuleOneRequestHandler>();

        var ioc = services.BuildServiceProvider();

        ioc.StartRebus();

        return ioc;
    }
}