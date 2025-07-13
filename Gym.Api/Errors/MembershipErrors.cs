using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class MembershipErrors
{
    public static Error NotFound => new("Memberships.NotFound", "Not found any Memberships", StatusCodes.Status404NotFound);
    public static Error NotCompeleteSubscription => new("Memberships.NotCompelete Subscription", "Not Compelete Subscription", StatusCodes.Status404NotFound);
    public static Error AlreadyExists => new("Memberships.AlreadyExists", "Membership already exists", StatusCodes.Status409Conflict);
    public static Error InvalidPlanType => new("Memberships.InvalidPlanType", "Invalid Membership type", StatusCodes.Status400BadRequest);
    public static Error InvalidAutoRenewal => new("Memberships.InvalidAutoRenewal", "This plan does not support auto-renewal", StatusCodes.Status400BadRequest);
}
