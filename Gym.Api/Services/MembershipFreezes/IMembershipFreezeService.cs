using Gym.Api.Abstractions;

namespace Gym.Api.Services.MembershipFreezes;

public interface IMembershipFreezeService
{
    Task<Result> AddAsync(int memberId,int membershipId, CancellationToken cancellation = default);
}
