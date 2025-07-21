using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Admin)]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPut("{userId}/unlock")]
    public async Task<IActionResult> Unlock(string userId)
    {
        var result=await _userService.UnloackAsync(userId);

       return result.IsSuccess? Ok(): result.ToProblem();
    }
}
