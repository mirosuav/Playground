using System.Collections.Concurrent;
using System.Threading;

namespace AsyncLock;

/// <summary>
/// Keeps the Id (Guid) of current async control flow regardless of the running thread.
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1?view=net-8.0"/>
/// </summary>
internal static class AsyncControlFlowContext
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


public static class AsyncLock
{
    private static readonly object selfLock = new();
    private static readonly ConcurrentDictionary<object, LockedSectionContext> protectedSections = new();

    internal static int ProtectedSectionsCount => protectedSections.Count();

    /// <summary>
    /// Asynchronously waits for the protected section to be freed and returns IDisposable object
    /// that locks the section until disposal
    /// </summary>
    /// <param name="key">Key for locking the section</param>
    /// <param name="cancellationToken">Cancellation token used to cancel waiting</param>
    /// <returns>IDisposable object that locks the section until disposal</returns>
    public static Task<IDisposable> WaitAsync(object key, CancellationToken cancellationToken = default)
    {
        lock (selfLock)
        {
            return protectedSections.TryGetValue(key, out var lockedContext)
                ? EnterProtectedSection(lockedContext, cancellationToken)
                : CreateNewProtectedSection(key, cancellationToken);
        }
    }

    private static Task<IDisposable> EnterProtectedSection(
        LockedSectionContext protectedSectionContext,
        CancellationToken cancellationToken)
    {
        // Same context is reentering its own protected section
        // meaning its not blocked and doesn't need to release a semaphore
        if (protectedSectionContext.ContextId == AsyncControlFlowContext.GetId())
        {
            return Task.FromResult((IDisposable)new DisposedLock());
        }

        return CreateNewLockedSectionReleaser(protectedSectionContext, cancellationToken);
    }

    private static Task<IDisposable> CreateNewProtectedSection(
        object key,
        CancellationToken cancellationToken)
    {
        var protectedSectionContext = new LockedSectionContext(AsyncControlFlowContext.GetId());

        if (!protectedSections.TryAdd(key, protectedSectionContext))
            throw new InvalidOperationException($"Could not asynchronously lock section on {key}.");

        return CreateNewLockedSectionReleaser(protectedSectionContext, cancellationToken);
    }

    private static Task<IDisposable> CreateNewLockedSectionReleaser(
        LockedSectionContext protectedSectionContext,
        CancellationToken cancellationToken)
    {
        // New async context is entering locked section
        IDisposable sectionReleaser = new LockedSectionReleaser(protectedSectionContext.Semaphore);

        Task waiter = protectedSectionContext.Semaphore.WaitAsync(cancellationToken);

        // Semaphore is open, don't wait just enter with releaser
        if (waiter.IsCompleted)
            return Task.FromResult(sectionReleaser);

        // Return awaiting semaphore task with continuation
        // When semaphore is released return section releaser
        // which releases the semaphore and protected section on disposal
        return waiter.ContinueWith(
                (_, sectionReleaser) => sectionReleaser as IDisposable,
                sectionReleaser,
                cancellationToken)!;
    }

    private sealed class LockedSectionContext(Guid contextId)
    {
        public readonly Guid ContextId = contextId;
        public readonly SemaphoreSlim Semaphore = new(1, 1);
    }

    private sealed class LockedSectionReleaser(SemaphoreSlim Semaphore) : IDisposable
    {
        private readonly SemaphoreSlim semaphore = Semaphore;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // TODO can we remove the section from the dictionary here ?
            if (disposing) semaphore.Release();
        }

        ~LockedSectionReleaser()
        {
            Dispose(false);
        }
    }

    // TODO Check if we can return a static disposed instance of this instead of newing every time
    private sealed class DisposedLock : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);
    }
}


