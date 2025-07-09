using Gym.Api.Abstractions;
using Gym.Api.Contracts.Authentications;

namespace Gym.Api.Services.Auth;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
}
