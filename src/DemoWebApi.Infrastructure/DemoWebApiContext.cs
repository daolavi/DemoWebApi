using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Infrastructure;

public class DemoWebApiContext : DbContext
{
    public DbSet<DemoTask> DemoTasks { get; init; }

    public DemoWebApiContext() { }

    public DemoWebApiContext(DbContextOptions<DemoWebApiContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoWebApiContext).Assembly);
    }
}