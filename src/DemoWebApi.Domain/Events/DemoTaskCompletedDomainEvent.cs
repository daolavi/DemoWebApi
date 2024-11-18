using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using MediatR;

namespace DemoWebApi.Domain.Events;

public class DemoTaskCompletedDomainEvent(DemoTask demoTask) : INotification
{
    public DemoTask DemoTask { get; } = demoTask;
}