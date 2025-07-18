using Gym.Api.Abstractions;
using Gym.Api.Contracts.Users;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Gym.Api.Services.Users;

public class UserService(UserManager<ApplicationUser> userManager): IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result> UpdateAsync(string userId, UpdateUserRequest request)
    {
        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        user = request.Adapt(user);

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();


        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
