using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using FluentResults;

namespace DemoWebApi.Tests.Common;

public class DemoTaskBuilder
{
    private string? _name;
    private DateTime? _dueDate;
    private DateTime? _completedDate;

    public DemoTaskBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public DemoTaskBuilder WithDueDate(DateTime? dueDate)
    {
        _dueDate = dueDate;
        return this;
    }

    public DemoTaskBuilder WithCompletedDate(DateTime? completedDate)
    {
        _completedDate = completedDate;
        return this;
    }

    public Result<DemoTask> Build()
    {
        var demoTask = DemoTask.CreateTask(_name, _dueDate, _completedDate);
        return demoTask;
    }
}