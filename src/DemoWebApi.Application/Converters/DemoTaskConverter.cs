using DemoWebApi.Contracts.Responses;
using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;

namespace DemoWebApi.Application.Converters;

public static class DemoTaskConverter
{
    public static DemoTaskDto ToContract(this DemoTask demoTask)
        => new DemoTaskDto(demoTask.Id, demoTask.Name, demoTask.IsDone, demoTask.DueDate, demoTask.CompletionDate);
}