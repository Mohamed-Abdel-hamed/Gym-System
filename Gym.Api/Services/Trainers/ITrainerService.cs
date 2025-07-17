using Gym.Api.Abstractions;
using Gym.Api.Contracts.Trainers;

namespace Gym.Api.Services.Trainers;

public interface ITrainerService
{
    Task<Result<TrainerResponse>> GetAsync(string userId,CancellationToken cancellation=default); 
}
