using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;

namespace RunWorkflowApp;

internal class Program
{
    private static ServiceProvider _ioc;

    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddWorkflow(x => x.UseSqlite(@"Data Source=workflow.db;", true));
        services.AddTransient<ExternalService>();
        //services.AddTransient<RecurringStep>();
        services.AddTransient<RepeatUntilWorkDone>();

        _ioc = services.BuildServiceProvider();

        try
        {
            await RunWorkflow();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await _ioc.DisposeAsync();
        }
    }

    private static async Task RunWorkflow()
    {
        if (File.Exists("workflow.db"))
            File.Delete("workflow.db");

        var workflowHost = _ioc.GetRequiredService<IWorkflowHost>();
        workflowHost.RegisterWorkflow<MyWorkflow, WorkState>();

        workflowHost.Start();
        
        await workflowHost.StartWorkflow(nameof(MyWorkflow), 1, new WorkState());

        Console.ReadLine();

        workflowHost.Stop();
    }
}