using Gym.Api.Abstractions;
using Gym.Api.Contracts.Roles;
using Gym.Api.Errors;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.Roles;

public class RoleService(RoleManager<IdentityRole> roleManager) : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    public async Task<Result<IEnumerable<RoleResponse>>> GetAllAsync()
    {
      var roles=  await _roleManager.Roles
            .AsNoTracking()
            .ProjectToType<RoleResponse>()
            .ToListAsync();
        if (roles.Count == 0)
            return Result.Failure<IEnumerable<RoleResponse>>(RoleErrors.NotFound);

        return Result.Success<IEnumerable<RoleResponse>>(roles);

    }
    public async Task<Result<RoleResponse>> GetAsync(string roleId)
    {
        var role = await _roleManager.Roles
              .AsNoTracking()
              .SingleOrDefaultAsync(r=>r.Id==roleId);


        if (role is null)
            return Result.Failure<RoleResponse>(RoleErrors.NotFound);

        var response = role.Adapt<RoleResponse>();

        return Result.Success(response);

    }

}
