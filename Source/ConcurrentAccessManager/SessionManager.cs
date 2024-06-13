using System.Collections.Concurrent;

namespace ConcurrentAccessManager;

public sealed class SessionManager
{
    private class ResourceSessions
    {
        private readonly Dictionary<Guid, int> _sessions = new();
        public SemaphoreSlim Semaphore { get; } = new(1, 1);

        public int BookedCount => _sessions.Sum(x => x.Value);

        public Guid Add(int requestedQuantity)
        {
            var id = Guid.NewGuid();
            _sessions.Add(id, requestedQuantity);
            return id;
        }

        public bool Remove(Guid id)
        {
            return _sessions.Remove(id);
        }
    }

    private readonly static ConcurrentDictionary<string, ResourceSessions> _sessions = new();

    public int GetBookedCount(string resourceName)
    {
        return _sessions.TryGetValue(resourceName, out var sessions)
            ? sessions.BookedCount
            : 0;
    }

    public Task LockAsync(string resourceName)
    {
        return _sessions
            .GetOrAdd(resourceName, new ResourceSessions())
            .Semaphore
            .WaitAsync();
    }

    public int Unlock(string resourceName)
    {
        return _sessions
            .GetOrAdd(resourceName, new ResourceSessions())
            .Semaphore
            .Release();
    }

    public Guid NewSession(string resourceName, int requestedQuantity)
    {
        return _sessions
            .GetOrAdd(resourceName, new ResourceSessions())
            .Add(requestedQuantity);
    }

    public void Commit(string resourceName, Guid Id)
    {
        var removed = _sessions.TryGetValue(resourceName, out var sessions)
            ? sessions.Remove(Id)
            : false;

        if (!removed)
            throw new InvalidOperationException($"No session with Id: {Id} was registered for resource: {resourceName}.");
    }
}
