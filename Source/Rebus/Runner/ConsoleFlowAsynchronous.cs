using ModuleOne;
using Rebus.Bus;
using Rebus.Handlers;
using System.Collections.Concurrent;

namespace Runner;

public static class ConsoleFlowAsynchronous
{
    public static TaskCompletionSource<bool> ResponsesReceivedCompletion = new();
    public static ConcurrentDictionary<int, Response1> ModuleOneResponses = new();

    public static async Task Run(IBus bus)
    {
        Console.WriteLine($">>>>> Starting {nameof(ConsoleFlowAsynchronous)}...");
        
        var requestTasks = Enumerable.Range(0, Program.RequestsCount)
            .Select(i => bus.Send(new Command1(i, 100)))
            .ToList();

        await Task.WhenAll(requestTasks);

        await ResponsesReceivedCompletion.Task;

        Console.WriteLine($">>>>> Host received {ModuleOneResponses.Count} responses...");
    }
}

public class Response1Handler(IBus bus) : IHandleMessages<Response1>
{
    public Task Handle(Response1 message)
    {
        ConsoleFlowAsynchronous.ModuleOneResponses.TryAdd(message.I, message);

        if (ConsoleFlowAsynchronous.ModuleOneResponses.Count == Program.RequestsCount)
            ConsoleFlowAsynchronous.ResponsesReceivedCompletion.SetResult(true);

        return Task.CompletedTask;
    }
}

