using FluentResults;

namespace DemoWebApi.Domain.AggregatesModel.AuditLogAggregate;

public class AuditLog : Entity
{
    public string Action { get; private set; }

    private AuditLog(string action)
    {
        Action = action;
    }

    public static Result<AuditLog> Create(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            return Result.Fail<AuditLog>("Action is required");
        }
        
        var auditLog = new AuditLog(action);
        return Result.Ok(auditLog);
    }
}