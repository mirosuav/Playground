using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace RunWorkflowApp;

public class ExternalService(IWorkflowHost workflowHost)
{
    private static int _itemsRemaining = 6;

    public Task DoWork(Guid id)
    {
        Console.WriteLine($"DoWork {id}");
        _ = WaitAndSendEvent(id);
        return Task.CompletedTask;
    }

    private async Task WaitAndSendEvent(Guid id)
    {
        await Task.Delay(100);

        try
        {
            if (_itemsRemaining > 0)
                Interlocked.Decrement(ref _itemsRemaining);

            await workflowHost.PublishEvent(
                 nameof(RecurringResult),
                 id.ToString(),
                 new RecurringResult(_itemsRemaining)
             );
            Console.WriteLine($"RecurringResult event published [id: {id}, remaining: {_itemsRemaining}]");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}

