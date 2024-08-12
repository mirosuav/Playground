using WorkflowCore.Exceptions;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace RunWorkflowApp;
public class WorkState
{
    public Guid RecordId { get; set; }
    public object ExternalData { get; set; }

    public bool HasRecurringResult => ExternalData is RecurringResult;

    public int RemainingItems => ExternalData is RecurringResult rr ? rr.RemainingItems : 0;

    public override string ToString() => $"State:[ ExternalData={ExternalData} ]";
}

public class MyWorkflow : IWorkflow<WorkState>
{
    public string Id => nameof(MyWorkflow);
    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkState> builder)
    {
        builder
            .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
            .StartWith(state =>
            {
                Console.WriteLine($"Starting workflow: {state.Workflow.Data}");
                return ExecutionResult.Next();
            })
            //.Then<RepeatUntilWorkDone>()


            .Output(state => state.ExternalData,
                step => new RecurringResult(1)) // start with at least one item to process
            .While(state => state.HasRecurringResult && state.RemainingItems > 0)
            .Do(x =>
            {
                x.StartWith(state =>
                    {
                        Console.WriteLine($"> Recur iteration ENTER: {state.Workflow.Data}");
                        return ExecutionResult.Next();
                    })
                    .Then<RecurringStep>()
                    .Output(state => state.RecordId, step => step.Id)
                    .WaitFor(nameof(RecurringResult), state => state.RecordId.ToString(), state => DateTime.Now)
                    .Output(state => state.ExternalData, waitFor => waitFor.EventData)
                    .Then(state =>
                    {
                        Console.WriteLine($"> Recur iteration EXIT: {state.Workflow.Data}");
                        return ExecutionResult.Next();
                    });
            })

            //.Recur(
            //    state => TimeSpan.FromMilliseconds(100),
            //    state => state.HasRecurringResult && state.RemainingItems == 0)
            //.Do(x =>
            //{
            //    x.StartWith(state =>
            //        {
            //            Console.WriteLine($"> Recur iteration ENTER: {state.Workflow.Data}");
            //            return ExecutionResult.Next();
            //        })
            //        .Then<RecurringStep>()
            //        .Output(state => state.RecordId, step => step.Id)
            //        .Output(state => state.ExternalData, step => null) // Reset external data
            //        .WaitFor(nameof(RecurringResult), state => state.RecordId.ToString(), state => DateTime.Now)
            //        .Output(state => state.ExternalData, waitFor => waitFor.EventData)
            //        .Then(state =>
            //        {
            //            Console.WriteLine($"> Recur iteration EXIT: {state.Workflow.Data}");
            //            return ExecutionResult.Next();
            //        });
            //})

            .Then(state =>
            {
                Console.WriteLine($"Workflow finished: {state.Workflow.Data}");
                return ExecutionResult.Next();
            });

    }
}

public class RepeatUntilWorkDone(ExternalService myService) : StepBodyAsync
{
    public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        Console.WriteLine($"Entering RepeatUntilWorkDone: {context.ExecutionPointer.EventData}");

        var now = DateTime.Now; // capture event published after this point

        if (!context.ExecutionPointer.EventPublished)
        {
            var id = await CreateWorkerAndDoWork();
            return WaitForRecurringResultEvent(id, now);
        }

        if (context.ExecutionPointer.EventData is not RecurringResult recurringResult)
            throw new ApplicationException("Invalid or missing data returned from recurring command!");

        if (recurringResult.RemainingItems <= 0)
            return ExecutionResult.Next();

        var id2 = await CreateWorkerAndDoWork();
        return WaitForRecurringResultEvent(id2, now);
    }

    private static ExecutionResult WaitForRecurringResultEvent(Guid id, DateTime after)
    {
        return ExecutionResult.WaitForEvent(nameof(RecurringResult), id.ToString(), after);
    }

    private async Task<Guid> CreateWorkerAndDoWork()
    {
        var id = Guid.NewGuid();
        Console.WriteLine("Entering RecurringStep...");
        await myService.DoWork(id);
        return id;
    }

}


public record RecurringResult(int RemainingItems);

public class RecurringStep(ExternalService myService) : StepBodyAsync
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        Console.WriteLine("Entering RecurringStep...");
        await myService.DoWork(Id);
        return ExecutionResult.Next();
    }
}