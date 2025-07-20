using Gym.Api.Abstractions;
using Gym.Api.Contracts.Authentications;
using Gym.Api.Contracts.Staffs;
using Gym.Api.Contracts.Trainers;
using Gym.Api.Contracts.Users;
using Gym.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthsController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellation)
    {
        var result = await _authService.GetTokenAsync(request.Email,request.Password, cancellation);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request,CancellationToken cancellation)
    {
        var result=await _authService.RegisterAsync(request,cancellation);

            return result.IsSuccess? Ok():result.ToProblem();
    }
    [HttpPost("register-trainer")]
    public async Task<IActionResult> RegisterTrainer([FromBody] RegisterTrainerRequest request, CancellationToken cancellation)
    {
        var result = await _authService.RegisterTrainerAsync(request, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("register-staff")]
    public async Task<IActionResult> RegisterStaff([FromBody] RegisterStaffRequest request, CancellationToken cancellation)
    {
        var result = await _authService.RegisterStaffAsync(request, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await _authService.ConfirmEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
    {
        var result=await _authService.SendResetPasswordAsync(request);
        return result.IsSuccess? Ok() : result.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {

        var authResult = await _authService.ResetPasswordAsync(request);
        return authResult.IsSuccess ? Ok() : authResult.ToProblem();
    }
}
