namespace Gym.Api.Contracts.Classes;

public record ClassResponse
    (
    string Title,
    DateTime StartDate,
    TimeSpan Duration,
    int Capacity,
    string TrainerName
    );
