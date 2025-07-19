namespace Gym.Api.Contracts.Users;

public record ResetPasswordRequest
    (
      string Email,
      string Token,
      string NewPassword
    );
