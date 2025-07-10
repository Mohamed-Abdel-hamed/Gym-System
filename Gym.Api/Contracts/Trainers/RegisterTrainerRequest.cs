using Gym.Api.Contracts.Authentications;

namespace Gym.Api.Contracts.Trainers;

public record RegisterTrainerRequest
    (
    RegisterRequest Info,
    DateOnly HireDate
    );
