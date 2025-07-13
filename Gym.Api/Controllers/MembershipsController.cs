using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Memberships;
using Gym.Api.Services.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Api.Controllers;
[Route("api/SubscriptionPlane/{planeId}/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Member)]
public class MembershipsController(IMembershipService _membershipService) : ControllerBase
{
    private readonly IMembershipService _membershipService = _membershipService;
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromRoute]int planeId,[FromBody] MembershipRequest request, CancellationToken cancellation = default)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var result=await _membershipService.SubscribeAsync(planeId,request.AutoRenew,userId,cancellation);
        return result.IsSuccess? Ok(new { url = result.Value }): result.ToProblem();
    }
    [HttpGet("success")]
    public async Task<IActionResult> Success(int id)
    {
        var result = await _membershipService.SuccessAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("error")]
    public  IActionResult Error()
    {
        return Ok("Error Subscribe");
    }

}
