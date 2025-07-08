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
}
