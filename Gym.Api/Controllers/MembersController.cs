using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Member)]
public class MembersController(IMemberSerive memberSerive) : ControllerBase
{
    private readonly IMemberSerive _memberSerive = memberSerive;

    [HttpGet("")]
    public async Task<IActionResult> Get(CancellationToken cancellation=default)
    {
        var userId=User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var result = await _memberSerive.GetAsync(userId,cancellation);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
