using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public static class RoleErrors
{
    public static readonly Error NotFound =
    new("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);
}
