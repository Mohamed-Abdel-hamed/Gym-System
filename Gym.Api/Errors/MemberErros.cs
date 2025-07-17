using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class MemberErros
{
    public static Error NotFound => new("member.NotFound", "Not found any member", StatusCodes.Status404NotFound);
}
