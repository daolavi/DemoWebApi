using FluentResults;

namespace DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;

public class DemoTask : Entity
{
    public string Name { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? CompletionDate { get; private set; }

    private DemoTask(string name, bool isDone, DateTime dueDate, DateTime? completionDate)
    {
        Name = name;
        IsDone = isDone;
        DueDate = dueDate;
        CompletionDate = completionDate;
    }

    public static Result<DemoTask> CreateTask(string name, bool isDone, DateTime? dueDate, DateTime? completionDate)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Fail<DemoTask>($"Name is required.");
        }
        
        if (isDone && !completionDate.HasValue)
        {
            return Result.Fail<DemoTask>($"CompletionDate is required for a completed task.");
        }

        if (!dueDate.HasValue)
        {
            return Result.Fail<DemoTask>($"DueDate is required.");
        }

        var task = new DemoTask(name, isDone, dueDate.Value, completionDate);
        return Result.Ok(task);
    }
    
    public void Complete(DateTime completionDate)
    {
        IsDone = true;
        CompletionDate = completionDate;
    }
}