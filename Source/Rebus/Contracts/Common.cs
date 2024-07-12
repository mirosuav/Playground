using ProtoBuf.Meta;
using Rebus.Config;
using Rebus.Serialization;
using Rebus.Serialization.Json;
using Rebus.Workers.TplBased;

namespace Contracts;

public static class Constants
{
    public const string HostBus = nameof(HostBus);
    public const string ModuleOneBus = nameof(ModuleOneBus);
    public const string ModuleTwoBus = nameof(ModuleTwoBus);

    public static void CommonBusOptionsConfigurer(OptionsConfigurer configurer)
    {
        configurer.EnableSynchronousRequestReply();
        //configurer.SetNumberOfWorkers(10);
        configurer.SetMaxParallelism(10);
        //configurer.UseTplToReceiveMessages();
    }

    public static void CommonBusSerializerConfig(StandardConfigurer<ISerializer> configurer)
    {
       // configurer.UseProtobuf();
       // configurer.UseNewtonsoftJson();
    }
}