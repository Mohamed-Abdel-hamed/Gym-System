using Gym.Api.Abstractions;
using Gym.Api.Contracts.Roles;

namespace Gym.Api.Services.Roles;

public interface IRoleService
{
    Task<Result<IEnumerable<RoleResponse>>> GetAllAsync();
}
