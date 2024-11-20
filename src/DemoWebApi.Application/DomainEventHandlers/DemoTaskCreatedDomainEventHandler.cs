using DemoWebApi.Domain.AggregatesModel.AuditLogAggregate;
using DemoWebApi.Domain.Events;
using DemoWebApi.Infrastructure;
using MediatR;

namespace DemoWebApi.Application.DomainEventHandlers;

public class DemoTaskCreatedDomainEventHandler(DemoWebApiContext dbContext) 
    : INotificationHandler<DemoTaskCreatedDomainEvent>
{
    public async Task Handle(DemoTaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var auditLog = AuditLog.Create($"DemoTask {notification.DemoTask.Id} has been created");
        await dbContext.AuditLogs.AddAsync(auditLog.Value, cancellationToken);
    }
}