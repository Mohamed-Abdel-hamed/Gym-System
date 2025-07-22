using Gym.Api.Abstractions;
using Gym.Api.Contracts.Members;

namespace Gym.Api.Services.Members;

public interface IMemberSerive
{
    Task<Result<MemberResponse>> GetAsync(string userId,CancellationToken cancellation=default);
    Task<Result<string>> SearchAsync(string name,CancellationToken cancellation=default);
    Task<Result<MemberResponse>> DetailsAsync(string key,CancellationToken cancellation=default);
}
