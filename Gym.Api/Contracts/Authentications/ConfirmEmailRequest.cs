namespace Gym.Api.Contracts.Authentications;

public record ConfirmEmailRequest
    (
    string UserId,
    string Token
    );
