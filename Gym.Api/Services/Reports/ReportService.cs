using Gym.Api.Abstractions;
using Gym.Api.Contracts.Dashboards;
using Gym.Api.Persistence;
using Gym.Api.Services.Memberships;

namespace Gym.Api.Services.Reports;

public class ReportService(IMembershipService _membershipService) : IReportService
{
    private readonly IMembershipService _membershipService = _membershipService;

    public async Task<Result<DashboardSummary>> GetDashboardSummary()
    {
        var numberOfActiveMemberships=await _membershipService.GetNumberOfActiveMemberships();
        var numberOfExpiredMemberships = await _membershipService.GetNumberOfExpiredMemberships();

        DashboardSummary dashboardSummary=new(numberOfActiveMemberships, numberOfExpiredMemberships);

        return Result.Success(dashboardSummary);
    }
}
