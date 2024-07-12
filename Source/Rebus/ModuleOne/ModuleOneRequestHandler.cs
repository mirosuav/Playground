using ModuleTwo;
using Rebus;
using Rebus.Bus;
using Rebus.Handlers;

namespace ModuleOne;

public class ModuleOneRequestHandler(IBus bus) : IHandleMessages<ModuleOneRequest>
{
    public async Task Handle(ModuleOneRequest message)
    {
        // Console.WriteLine($">>>>> ModuleOneRequestHandler got: {message}");
        var result = await bus.SendRequest<int>(new ModuleTwoRequest(message.A, message.B));
        // Console.WriteLine($">>>>> ModuleOneRequestHandler replies: {result}");
        await bus.Reply(result);
    }
}

