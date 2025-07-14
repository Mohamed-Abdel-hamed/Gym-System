using Gym.Api.Abstractions;

namespace Gym.Api.Services.MembershipFreezes;

public interface IMembershipFreezeService
{
    Task<Result> AddAsync(string userId,int membershipId, CancellationToken cancellation = default);
    Task<Result> UnFreezeAsync(string userId, int membershipId, CancellationToken cancellation = default);
}
