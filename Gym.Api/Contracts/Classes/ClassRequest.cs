namespace Gym.Api.Contracts.Classes;

public record ClassRequest
    (
    string Title,
    int? Capacity,
    TimeSpan Duration
    );
