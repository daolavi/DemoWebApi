using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoWebApi.Infrastructure.EntityConfigurations;

public class DemoTaskEntityTypeConfiguration : IEntityTypeConfiguration<DemoTask>
{
    public void Configure(EntityTypeBuilder<DemoTask> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
    }
}