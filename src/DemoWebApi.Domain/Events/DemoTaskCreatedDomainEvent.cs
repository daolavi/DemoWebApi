using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using MediatR;

namespace DemoWebApi.Domain.Events;

public class DemoTaskCreatedDomainEvent(DemoTask demoTask) : INotification
{
    public DemoTask DemoTask { get; } = demoTask;
}