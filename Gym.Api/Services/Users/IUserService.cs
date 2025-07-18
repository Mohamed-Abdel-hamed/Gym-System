using Gym.Api.Abstractions;
using Gym.Api.Contracts.Users;

namespace Gym.Api.Services.Users;

public interface IUserService
{
    Task<Result> UpdateAsync(string userId, UpdateUserRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
