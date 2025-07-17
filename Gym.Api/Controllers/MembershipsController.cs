using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Memberships;
using Gym.Api.Services.Memberships;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gym.Api.Controllers;
[Route("api/SubscriptionPlane/{planeId}/[controller]")]
[ApiController]
public class MembershipsController(IMembershipService _membershipService) : ControllerBase
{
    private readonly IMembershipService _membershipService = _membershipService;
    [Authorize(Roles = AppRoles.Member)]
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromRoute]int planeId,[FromBody] MembershipRequest request, CancellationToken cancellation = default)
    {
        var userId= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var result=await _membershipService.SubscribeAsync(planeId,request.AutoRenew,userId,cancellation);
        return result.IsSuccess? Ok(new { url = result.Value }): result.ToProblem();
    }
    [Authorize(Roles = AppRoles.Member)]
    [HttpGet("success")]
    public async Task<IActionResult> Success(int id)
    {
        var result = await _membershipService.SuccessAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [Authorize(Roles = AppRoles.Member)]
    [HttpGet("error")]
    public  IActionResult Error()
    {
        return Ok("Error Subscribe");
    }
    [Authorize(Roles = AppRoles.Staff)]
    [HttpGet("alert-to-expiresmember")]
    public async Task<IActionResult> AlertToExpiresMember([FromRoute] int planeId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        RecurringJob.AddOrUpdate(() =>_membershipService.AlertToExpiresMember(userId, planeId), Cron.Daily);
        return Ok();
    }

}
