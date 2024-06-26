using System.Collections.Concurrent;
using System.Threading;

namespace AsyncLock;

internal static class AsynchronousControlFlowContext
{
    private readonly static AsyncLocal<Guid> CurrentId = new();

    public static Guid GetId()
    {
        if (CurrentId.Value == default)
        {
            CurrentId.Value = Guid.NewGuid();
        }

        return CurrentId.Value;
    }
}

/// <summary>
/// Represent currently locked asynchronous control flow section
/// </summary>
internal sealed class LockingContext(Guid contextId)
{
    public readonly Guid ContextId = contextId;
    public readonly SemaphoreSlim Semaphore = new(1, 1);
}

public sealed class AsyncLock
{
    private static readonly SemaphoreSlim selfLock = new SemaphoreSlim(1, 1);
    private static readonly ConcurrentDictionary<object, LockingContext> locks = new();

    public async Task<IDisposable> LockAsync(object key, CancellationToken cancellation = default)
    {
        await selfLock.WaitAsync(cancellation);

        try
        {
            // Get Id of current asynchronous flow context
            var currentContextId = AsynchronousControlFlowContext.GetId();

            // Entering already locked section
            if (locks.TryGetValue(key, out var lockedSection))
            {
                // Same context is reentering its own locked section
                if (lockedSection.ContextId == currentContextId)
                {

                }

                // New context is entering locked section                
                var releaser = new LockReleaser(lockedSection.Semaphore);
                var waiter = lockedSection.Semaphore.WaitAsync(cancellation);

                // Semaphore is already released
                if (waiter.IsCompleted)
                    return Task.FromResult(releaser);

                // Return pending semaphore task and return releaser on continuation
                return waiter!.ContinueWith(
                        (t, state) => state as IDisposable,
                        releaser,
                        cancellation,
                        TaskContinuationOptions.ExecuteSynchronously,
                        TaskScheduler.Default);
            }

            // Entering new unlocked section

        }
        finally
        {
            selfLock.Release();
        }
    }
}

internal sealed class LockReleaser(SemaphoreSlim Semaphore) : IDisposable
{
    private readonly SemaphoreSlim semaphore = Semaphore;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing) semaphore.Release();
    }

    ~LockReleaser()
    {
        Dispose(false);
    }
}


