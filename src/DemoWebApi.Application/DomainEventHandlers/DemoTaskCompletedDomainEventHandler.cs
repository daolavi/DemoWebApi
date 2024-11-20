using DemoWebApi.Domain.AggregatesModel.AuditLogAggregate;
using DemoWebApi.Domain.Events;
using DemoWebApi.Infrastructure;
using MediatR;

namespace DemoWebApi.Application.DomainEventHandlers;

public class DemoTaskCompletedDomainEventHandler(DemoWebApiContext dbContext) 
    : INotificationHandler<DemoTaskCompletedDomainEvent>
{
    public async Task Handle(DemoTaskCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var auditLog = AuditLog.Create($"DemoTask {notification.DemoTask.Id} has been completed");
        await dbContext.AuditLogs.AddAsync(auditLog.Value, cancellationToken);
    }
}