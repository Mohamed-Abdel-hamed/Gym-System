namespace Gym.Api.Contracts.Authentications;

public record AuthResponse
(
    string Id,
    string?Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn
    );

