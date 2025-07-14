using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class MembershipFreezeErrors
{
    public static Error NotAllowedFreeze => new("Memberships. NotAllowedFreeze", "Remaining subscription period memberships less than max freeze days of plan", StatusCodes.Status400BadRequest);
    public static Error NotExistsFreeze => new("Memberships. NotExistsFreeze", "Not exists freeze to this plane", StatusCodes.Status404NotFound);
    public static Error NotAllowedFreezeCount => new("Memberships. NotAllowedFreezeCount", " Not allowed  freeze memberships  count of freeze greater than max freeze per year", StatusCodes.Status400BadRequest);
}
