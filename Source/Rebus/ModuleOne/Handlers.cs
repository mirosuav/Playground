using ModuleTwo;
using Rebus;
using Rebus.Bus;
using Rebus.Handlers;

namespace ModuleOne;

/// <summary>
/// Handle query from Host resend it to ModuleTwo, wait for response and reply result back to host
/// </summary>
public class Query1Handler(IBus bus) : IHandleMessages<Query1>
{
    public async Task Handle(Query1 message)
    {
        Console.WriteLine($"[#######] Received: {message}");
        var result = await bus.SendRequest<int>(new Query2(message.A, message.B));
        await bus.Reply(result);
    }
}

/// <summary>
/// Handle command from Host and resend it to ModuleTwo
/// </summary>
public class Command1Handler(IBus bus) : IHandleMessages<Command1>
{
    public Task Handle(Command1 message)
    {
        Console.WriteLine($"[#######] Received and sending to ModuleTwo: {message}");
        return bus.Send(new Command2(message.A, message.B));
    }
}

/// <summary>
/// Handle response from ModuleTwo and resend to Host
/// </summary>
public class Response2Handler(IBus bus) : IHandleMessages<Response2>
{
    public Task Handle(Response2 message)
    {
        Console.WriteLine($"[#######] Received and sending to Host: {message}");
        return bus.Send(new Response1(message.I, message.C));
    }
}

