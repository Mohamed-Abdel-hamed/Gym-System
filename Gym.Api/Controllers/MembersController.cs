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
public class MembersController(IMemberSerive memberSerive) : ControllerBase
{
    private readonly IMemberSerive _memberSerive = memberSerive;

    [Authorize(Roles = AppRoles.Member)]
    [HttpGet("")]
    public async Task<IActionResult> Get(CancellationToken cancellation=default)
    {
        var userId=User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var result = await _memberSerive.GetAsync(userId, cancellation);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id,CancellationToken cancellation = default)
    {

        var result = await _memberSerive.DetailsAsync(id, cancellation);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
