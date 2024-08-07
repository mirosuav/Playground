﻿using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
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
                    .Serialization(Constants.CommonBusSerializerConfig)
                    .Routing(config => config
                        .TypeBased()
                        .MapAssemblyDerivedFrom<IResponse2>($"{Constants.ModuleOneBus}-queue"));
            },
            key: Constants.ModuleTwoBus);

        services.AutoRegisterHandlersFromAssemblyOf<Command2Handler>();

        var ioc = services.BuildServiceProvider();

        ioc.StartRebus();

        return ioc;
    }
}