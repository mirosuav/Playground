using Rebus.Bus;
using Rebus.Handlers;

namespace ModuleOne;

public class ModuleOneSimpleRequestHandler(IBus bus) : IHandleMessages<ModuleOneSimpleRequest>
{
    public async Task Handle(ModuleOneSimpleRequest message)
    {
        //Console.WriteLine($">>>>> ModuleOneSimpleRequest handled: {message}");
        await bus.Reply(message.Arg + 1);
    }
}

