namespace Gym.Api.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool UnlimitedDailyEntries { get; set; }

    //  class‑booking limits
    public int MaxClassBookingsPerDay { get; set; } = 1;
    public int MaxClassBookingsInFuture { get; set; } = 3;

    // Freeze rules
    public int MaxFreezesPerYear { get; set; } = 2;
    public int MaxFreezeDays { get; set; } = 30;

}
