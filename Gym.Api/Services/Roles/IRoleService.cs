using Gym.Api.Abstractions;
using Gym.Api.Contracts.Roles;

namespace Gym.Api.Services.Roles;

public interface IRoleService
{
    Task<Result<IEnumerable<RoleResponse>>> GetAllAsync();
    Task<Result<RoleResponse>> GetAsync(string roleId);
    Task<Result> AddAsync(RoleRequest request);
    Task<Result> UpdateAsync(string roleId, RoleRequest request);
}
