namespace Gym.Api.Contracts.Classes;

public record ClassRequest
    (
    string Name,
    int? Capacity,
    TimeSpan Duration
    );
