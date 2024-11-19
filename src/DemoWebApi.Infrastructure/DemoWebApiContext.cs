using DemoWebApi.Domain.AggregatesModel.AuditLogAggregate;
using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Infrastructure;

public class DemoWebApiContext : DbContext
{
    public DbSet<DemoTask> DemoTasks { get; init; }
    public DbSet<AuditLog> AuditLogs { get; init; }

    private readonly IMediator _mediator;
    public DemoWebApiContext(DbContextOptions<DemoWebApiContext> options) : base(options) { }
    
    public DemoWebApiContext(DbContextOptions<DemoWebApiContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoWebApiContext).Assembly);
    }
    
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}