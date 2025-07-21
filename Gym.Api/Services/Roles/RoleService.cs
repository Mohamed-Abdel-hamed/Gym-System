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
    public async Task<Result> AddAsync(RoleRequest request)
    {
       var isExists=await _roleManager.RoleExistsAsync(request.Name);

        if(isExists)
            return Result.Failure(RoleErrors.DuplicatedRole);
        var result = await _roleManager.CreateAsync(new IdentityRole { Name = request.Name });

        if(result.Succeeded)
        return Result.Success(result);

        var error= result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> UpdateAsync(string roleId,RoleRequest request)
    {

        var isExists = await _roleManager.Roles.AnyAsync(r=>r.Name==request.Name && r.Id!=roleId);

        if (isExists)
            return Result.Failure(RoleErrors.DuplicatedRole);

        if(await _roleManager.FindByIdAsync(roleId) is not { } role)
            return Result.Failure(RoleErrors.NotFound);

        role.Name = request.Name;

        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
            return Result.Success(result);

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

}
