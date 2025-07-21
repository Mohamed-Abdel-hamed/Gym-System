using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public static class RoleErrors
{
    public static readonly Error NotFound =
    new("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedRole =
        new("Role.DuplicatedRole", "Another role with the same name is already exists", StatusCodes.Status409Conflict);
}
