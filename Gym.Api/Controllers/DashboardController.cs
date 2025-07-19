using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Memberships;
using Gym.Api.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Admin)]  
public class DashboardController(IMembershipService membershipService,
    IReportService reportService) : ControllerBase
{
    private readonly IMembershipService _membershipService = membershipService;
    private readonly IReportService _reportService = reportService;

    [HttpGet("memberships-per-day")]
    public async Task<IActionResult>MembershipsPerDay(DateTime? startDate, DateTime? endDate)
    {
        var result=await _membershipService.MembershipsPerDay(startDate, endDate);
        return result.IsSuccess? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("dashboard-overview")]
    public async Task<IActionResult> DashboardOverview()
    {
        var result = await _reportService.GetDashboardSummary();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
