using ModuleOne;
using Rebus;
using Rebus.Bus;

namespace Runner;

internal static class ConsoleFlowWaitForResponse
{
    public static async Task Run(IBus bus)
    {
        Console.WriteLine($">>>>> Starting {nameof(ConsoleFlowWaitForResponse)}...");

        var requestTasks = Enumerable.Range(0, Program.RequestsCount)
            .Select(i => bus.SendRequest<int>(new Query1(i, 100)))
            .ToList();

        var results = await Task.WhenAll(requestTasks);

        Console.WriteLine($">>>>> Host received {results.Length} responses...");
    }
}


