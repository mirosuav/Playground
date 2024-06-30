using Bogus;
using FluentAssertions;
using Xunit.Abstractions;

namespace AsyncLock.Tests;
public class AsyncControlFlowContextTests
{
    private readonly ITestOutputHelper testOutput;

    public AsyncControlFlowContextTests(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }

    private Guid[] results = new Guid[10];
    [Fact]
    public void GetId_ShouldReturnDifferntId_ForDifferentQueuedThreads()
    {
        // ASSERT
        Array.Clear(results, 0, 10);
        int threads = 10;
        testOutput.WriteLine($"Running for {threads} threads");
        int[] data = Enumerable.Range(0, threads).ToArray();

        // ACT
        foreach (var idx in data)
        {
            ThreadPool.QueueUserWorkItem(RunIntensiveOperation, idx, false);
        }

        // ASSERT
        results.Distinct().Should().HaveCount(threads);
    }

    private void RunIntensiveOperation(int idx)
    {
        results[idx] = AsyncControlFlowContext.GetId();
        testOutput.WriteLine(
           $"""
            Processing item: {idx}, 
                AsyncControlFlowContext.GetId: {results[idx]}, 
                Thread: {Environment.CurrentManagedThreadId} 
            """
            );
    }


}