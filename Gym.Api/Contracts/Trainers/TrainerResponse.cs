using Gym.Api.Contracts.Classes;
using Gym.Api.Contracts.Users;

namespace Gym.Api.Contracts.Trainers;

public record TrainerResponse
    (
    UserProfileResponse User,
    DateOnly HireDate,
    IEnumerable<ClassWithTrainerResponse> Classes
    );