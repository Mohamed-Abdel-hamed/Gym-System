namespace Gym.Api.Contracts.SubscriptionPlans;

public record SubscriptionPlanResponse
    (
     string Name,
     bool UnlimitedDailyEntries,
     int MaxClassBookingsPerDay ,
     int MaxClassBookingsInFuture,
     int MaxFreezesPerYear,
     int MaxFreezeDays
    );
