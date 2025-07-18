using Gym.Api.Abstractions;
using Gym.Api.Contracts.Users;
using Gym.Api.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountsController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpPut("info")]
    public async Task<IActionResult> Info(UpdateUserRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _userService.UpdateAsync(userId,request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
