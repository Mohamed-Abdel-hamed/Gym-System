using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public static class SubscriptionPlanError
{
    public static Error NotFound => new("SubscriptionPlans.NotFound", "Not found any SubscriptionPlans");
    public static Error AlreadyExists => new("SubscriptionPlans.AlreadyExists", "Subscription plan already exists");
    public static Error InvalidPlanType => new("SubscriptionPlans.InvalidPlanType", "Invalid subscription plan type");
    public static Error InvalidDuration => new("SubscriptionPlans.InvalidDuration", "Invalid subscription plan duration");
    public static Error InvalidPrice => new("SubscriptionPlans.InvalidPrice", "Invalid subscription plan price");
}
