using DemoWebApi.Domain.AggregatesModel.AuditLogAggregate;
using DemoWebApi.Domain.Events;
using DemoWebApi.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DemoWebApi.Application.DomainEventHandlers;

public class DemoTaskCreatedDomainEventHandler(ILogger<DemoTaskCreatedDomainEventHandler> logger,
    DemoWebApiContext dbContext) : INotificationHandler<DemoTaskCreatedDomainEvent>
{
    public async Task Handle(DemoTaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var auditLog = AuditLog.Create($"DemoTask {notification.DemoTask.Id} has been created");
        await dbContext.AuditLogs.AddAsync(auditLog.Value, cancellationToken);
    }
}