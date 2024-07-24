using Contracts;
using Rebus;
using Rebus.Bus;
using Rebus.Handlers;

namespace ModuleTwo;

/// <summary>
/// Handle Query from ModuleOne and send back to ModuleOne
/// </summary>
public class Query2Handler(IBus bus) : IHandleMessages<Query2>
{
    public Task Handle(Query2 message)
    {
        Console.WriteLine($"[#######] ModuleTwo received: {message} and replies to ModuleOne");
        return bus.Reply(message.A + message.B);
    }
}

/// <summary>
/// Handle command from ModuleTwo and send response back
/// </summary>
public class Command2Handler(IBus bus) : IHandleMessages<Command2>
{
    public Task Handle(Command2 message)
    {
        Console.WriteLine($"[#######] ModuleTwo received and sending to ModuleOne: {message}");
        return bus.Send(new Response2(message.A, message.A + message.B));
    }
}

