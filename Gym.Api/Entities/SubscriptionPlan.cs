namespace Gym.Api.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool UnlimitedDailyEntries { get; set; }
    public decimal Price { get; set; }

    public int DurationInDays { get; set; }

    public bool SupportsAutoRenewal { get; set; } = false;

    public int MaxClassBookingsPerDay { get; set; }
    public int MaxClassBookingsInFuture { get; set; }

    public int MaxFreezesPerYear { get; set; }
    public int MaxFreezeDays { get; set; }

}
