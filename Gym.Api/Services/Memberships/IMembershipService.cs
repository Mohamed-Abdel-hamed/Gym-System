using Gym.Api.Abstractions;

namespace Gym.Api.Services.Memberships;

public interface IMembershipService
{
    Task<Result<string>> SubscribeAsync(int planeId,bool autoRenew,string userId, CancellationToken cancellation = default);
    Task<Result<string>> SuccessAsync(int id);
}
