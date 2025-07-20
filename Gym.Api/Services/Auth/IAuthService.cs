using Gym.Api.Abstractions;
using Gym.Api.Contracts.Authentications;
using Gym.Api.Contracts.Staffs;
using Gym.Api.Contracts.Trainers;

namespace Gym.Api.Services.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email,string password, CancellationToken cancellation = default);
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default);
    Task<Result> RegisterTrainerAsync(RegisterTrainerRequest request, CancellationToken cancellation = default);
    Task<Result> RegisterStaffAsync(RegisterStaffRequest request, CancellationToken cancellation = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
    Task<Result> SendResetPasswordAsync(ForgetPasswordRequest request);
}
