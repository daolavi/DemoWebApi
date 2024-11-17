using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Infrastructure;

public interface IDemoWebApiContext
{
    DbSet<DemoTask> DemoTasks { get; init; }
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}

public class DemoWebApiContext(DbContextOptions<DemoWebApiContext> options) : DbContext(options), IDemoWebApiContext
{
    public DbSet<DemoTask> DemoTasks { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoWebApiContext).Assembly);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        _ = await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}