using DemoWebApi.Domain.Events;
using FluentResults;

namespace DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;

public class DemoTask : Entity
{
    public string Name { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? CompletionDate { get; private set; }
    
    public bool IsDone => CompletionDate.HasValue;

    private DemoTask(string name, DateTime dueDate, DateTime? completionDate)
    {
        Name = name;
        DueDate = dueDate;
        CompletionDate = completionDate;
        
        AddDomainEvent(new DemoTaskCreatedDomainEvent(this));
    }

    public static Result<DemoTask> CreateTask(string? name, DateTime? dueDate, DateTime? completionDate)
    {
        var errorMessages = new List<string>();
        if (string.IsNullOrEmpty(name))
        {
            errorMessages.Add("Name is required.");
        }
        
        if (!dueDate.HasValue)
        {
            errorMessages.Add("DueDate is required.");
        }

        if (errorMessages.Count > 0)
        {
            return Result.Fail<DemoTask>(errorMessages);
        }

        var task = new DemoTask(name!, dueDate!.Value, completionDate);
        return Result.Ok(task);
    }
    
    public Result<bool> Complete(DateTime completionDate)
    {
        if (IsDone)
        {
            return Result.Fail<bool>("Task is already completed.");
        }
        CompletionDate = completionDate;
        AddDomainEvent(new DemoTaskCompletedDomainEvent(this));
        return Result.Ok(true);
    }
}