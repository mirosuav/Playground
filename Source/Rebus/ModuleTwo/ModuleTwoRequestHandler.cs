using Contracts;
using Rebus;
using Rebus.Bus;
using Rebus.Handlers;

namespace ModuleTwo;

public class ModuleTwoRequestHandler(IBus bus) : IHandleMessages<ModuleTwoRequest>
{
    public async Task Handle(ModuleTwoRequest message)
    {
        //Console.WriteLine($">>>>> ModuleTwoRequestHandler got: {message}");
        await bus.Reply(message.A + message.B);
    }
}

