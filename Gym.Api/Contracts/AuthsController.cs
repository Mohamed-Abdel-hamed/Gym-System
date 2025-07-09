using Gym.Api.Abstractions;
using Gym.Api.Contracts.Authentications;
using Gym.Api.Errors;
using Gym.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Contracts;
[Route("api/[controller]")]
[ApiController]
public class AuthsController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request,CancellationToken cancellation)
    {
        var result=await _authService.RegisterAsync(request,cancellation);

            return result.IsSuccess? Ok():result.ToProblem();
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
}
