using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AsyncLock.Tests;

public  class AsyncLockTests
{
    private readonly ITestOutputHelper testOutput;

    public AsyncLockTests(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }

    [Fact]
    public async Task OnContext_HasSameResults_WhenEntersSectionTwice()
    {
        // ARRANGE

        // ACT
        await ProtectedSection(1);
        await ProtectedSection(2);

        // ASSERT
        AsyncLock.ProtectedSectionsCount.Should().Be(1);

    }

    private static object lockKey = new();
    private async Task ProtectedSection(int idx)
    {
        using (var waiter = AsyncLock.WaitAsync(lockKey))
        {

            await Task.Delay(5);

            testOutput.WriteLine(
               $"""
            Processing item: {idx}, 
                AsyncControlFlowContext.GetId: {AsyncControlFlowContext.GetId()}, 
                Task.CurrentId: {Task.CurrentId}, 
                Thread: {Environment.CurrentManagedThreadId} 
            """
                );
        }
    }
}

