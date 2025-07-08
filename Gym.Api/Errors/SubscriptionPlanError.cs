using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public static class SubscriptionPlanError
{
    public static Error NotFound => new("SubscriptionPlans.NotFound", "Not found any SubscriptionPlans",StatusCodes.Status404NotFound);
    public static Error AlreadyExists => new("SubscriptionPlans.AlreadyExists", "Subscription plan already exists",StatusCodes.Status409Conflict);
    public static Error InvalidPlanType => new("SubscriptionPlans.InvalidPlanType", "Invalid subscription plan type", StatusCodes.Status400BadRequest);
    public static Error InvalidDuration => new("SubscriptionPlans.InvalidDuration", "Invalid subscription plan duration", StatusCodes.Status400BadRequest);
    public static Error InvalidPrice => new("SubscriptionPlans.InvalidPrice", "Invalid subscription plan price", StatusCodes.Status400BadRequest);
}
