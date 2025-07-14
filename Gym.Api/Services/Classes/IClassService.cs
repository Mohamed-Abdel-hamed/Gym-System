using Gym.Api.Abstractions;
using Gym.Api.Contracts.Classes;

namespace Gym.Api.Services.Classes;

public interface IClassService
{
    Task<Result> AddAsync(int trainerId, ClassRequest request,CancellationToken cancellation=default);
}
