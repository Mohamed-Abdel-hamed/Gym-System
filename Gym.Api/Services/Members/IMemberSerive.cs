using Gym.Api.Abstractions;
using Gym.Api.Contracts.Members;

namespace Gym.Api.Services.Members;

public interface IMemberSerive
{
    Task<Result<MemberResponse>> GetAsync(string userId,CancellationToken cancellation=default);
}
