using Gym.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Api.Persistence.EntitiesConfiguration;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(100);
        builder.HasIndex(x => new {x.TrainerId,x.Title, x.StartDate }).IsUnique();

    }
}
