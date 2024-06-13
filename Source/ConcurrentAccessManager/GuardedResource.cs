using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentAccessManager;


internal class GuardedResource
{
    public static string[] Resources = ["Resource 1", "Resource 2", "Resource 3", "Resource 4"];

    private static readonly ConcurrentDictionary<string, (int, int)> _resources =
        new ConcurrentDictionary<string, (int, int)>(new Dictionary<string, (int, int)>
        {
            { "Resource 1", (0,100) },
            { "Resource 2", (0,100) },
            { "Resource 3", (0,100) },
            { "Resource 4", (0,100) }
        });

    private readonly SessionManager _sessionManager = new();


    public async Task<Guid?> RequestResourceUsage(string resourceName, int requestedQuantity)
    {
        await _sessionManager.LockAsync(resourceName);

        try
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} books {resourceName} with quantity of {requestedQuantity}.");

            var limit = GetResourceLimit(resourceName);

            if (limit > 0
                && GetResourcePersistedUsage(resourceName)
                    + _sessionManager.GetBookedCount(resourceName)
                    + requestedQuantity
                    > limit)
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} exceeds {resourceName} limit: {limit}.");
                return null;
            }

            var sessionId =  _sessionManager.NewSession(resourceName, requestedQuantity);

            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} acquired session: {sessionId} for resource {resourceName} with quantity of {requestedQuantity}.");
           
            return sessionId;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            _sessionManager.Unlock(resourceName);
        }
    }

    private int GetResourcePersistedUsage(string resourceName) =>
        _resources.TryGetValue(resourceName, out var r)
        ? r.Item1
        : 0;

    private int GetResourceLimit(string resourceName) =>
        _resources.TryGetValue(resourceName, out var r)
        ? r.Item2
        : 0;

}

