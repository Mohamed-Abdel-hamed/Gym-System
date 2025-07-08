using Gym.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gym.Api.Persistence.EntitiesConfiguration;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(20);
        builder.Property(x => x.Description).HasMaxLength(100);
    }
}
