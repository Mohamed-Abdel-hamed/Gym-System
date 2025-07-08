namespace Gym.Api.Contracts.Authentications;

public record RegisterRequest(
    string Email,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName
);