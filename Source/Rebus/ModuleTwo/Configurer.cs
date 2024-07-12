using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Transport.InMem;

namespace ModuleTwo;

public static class Configurer
{
    public static ServiceProvider Configure(InMemNetwork inMemNetwork)
    {

        var services = new ServiceCollection();

        services.AddRebus(rebusConfigurer =>
            {
                return rebusConfigurer
                    .Transport(t => t.UseInMemoryTransport(inMemNetwork, $"{Constants.ModuleTwoBus}-queue"))
                    .Options(Constants.CommonBusOptionsConfigurer)
                    .Serialization(Constants.CommonBusSerializerConfig);
            },
            key: Constants.ModuleTwoBus);

        services.AutoRegisterHandlersFromAssemblyOf<ModuleTwoRequestHandler>();

        var ioc = services.BuildServiceProvider();

        ioc.StartRebus();

        return ioc;
    }
}