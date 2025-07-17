using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.MembershipFreezes;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class MembershipFreezesController(IMembershipFreezeService membershipFreezeService) : ControllerBase
{
    private readonly IMembershipFreezeService _membershipFreezeService = membershipFreezeService;
    [Authorize(Roles = AppRoles.Member)]
    [HttpPost("membershipId/{membershipId}/add-freeze")]
    public async Task<IActionResult> Add([FromRoute] int membershipId,CancellationToken cancellation=default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result=await _membershipFreezeService.AddAsync(userId, membershipId, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [Authorize(Roles = AppRoles.Member)]
    [HttpPost("membershipId/{membershipId}cancel")]
    public async Task<IActionResult> Cancel([FromRoute] int membershipId, CancellationToken cancellation = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _membershipFreezeService.UnFreezeAsync(userId, membershipId, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [Authorize(Roles = AppRoles.Staff)]
    [HttpGet("reactivate-frozen-memberships")]
    public IActionResult ReactivateFrozenMemberships()
    {
         //await _membershipFreezeService.ReactivateFrozenMemberships();
        RecurringJob.AddOrUpdate(()=> _membershipFreezeService.ReactivateFrozenMemberships(),Cron.Minutely);
        return Ok();
    }
}
