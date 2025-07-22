using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
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
    [HttpGet("{key}")]
    public async Task<IActionResult> Details(string key,CancellationToken cancellation = default)
    {

        var result = await _memberSerive.DetailsAsync(key, cancellation);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{name}-search")]
    public async Task<IActionResult> Serach(string name, CancellationToken cancellation = default)
    {

        var result = await _memberSerive.SearchAsync(name, cancellation);

        return result.IsSuccess ? RedirectToAction(nameof(Details), new { key = result.Value } ) : result.ToProblem();
    }
}
