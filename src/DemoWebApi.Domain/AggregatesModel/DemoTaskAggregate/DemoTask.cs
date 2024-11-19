using DemoWebApi.Domain.Events;
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
        
        AddDomainEvent(new DemoTaskCreatedDomainEvent(this));
    }

    public static Result<DemoTask> CreateTask(string? name, bool isDone, DateTime? dueDate, DateTime? completionDate)
    {
        var errorMessages = new List<string>();
        if (string.IsNullOrEmpty(name))
        {
            errorMessages.Add("Name is required.");
        }
        
        if (isDone && !completionDate.HasValue)
        {
            errorMessages.Add("CompletionDate is required for a completed task.");
        }

        if (!dueDate.HasValue)
        {
            errorMessages.Add("DueDate is required.");
        }

        if (errorMessages.Count > 0)
        {
            return Result.Fail<DemoTask>(errorMessages);
        }

        var task = new DemoTask(name!, isDone, dueDate!.Value, completionDate);
        return Result.Ok(task);
    }
    
    public void Complete(DateTime completionDate)
    {
        IsDone = true;
        CompletionDate = completionDate;
        
        AddDomainEvent(new DemoTaskCompletedDomainEvent(this));
    }
}