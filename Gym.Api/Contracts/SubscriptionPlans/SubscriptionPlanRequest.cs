namespace Gym.Api.Contracts.SubscriptionPlans;

public record SubscriptionPlanRequest
    (
        string Name,
        string Description,
        bool UnlimitedDailyEntries,
        decimal Price,
        int DurationInDays,
        bool SupportsAutoRenewal,
        int MaxClassBookingsPerDay,
        int MaxClassBookingsInFuture,
        int MaxFreezesPerYear,
        int MaxFreezeDays
    );
