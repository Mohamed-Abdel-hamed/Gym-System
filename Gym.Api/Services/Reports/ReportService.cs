using Gym.Api.Abstractions;
using Gym.Api.Contracts.Dashboards;
using Gym.Api.Persistence;
using Gym.Api.Services.Bookings;
using Gym.Api.Services.Memberships;

namespace Gym.Api.Services.Reports;

public class ReportService(IMembershipService _membershipService,IBookingService bookingService) : IReportService
{
    private readonly IMembershipService _membershipService = _membershipService;
    private readonly IBookingService _bookingService = bookingService;

    public async Task<Result<DashboardSummary>> GetDashboardSummary()
    {
        var numberOfActiveMemberships=await _membershipService.GetNumberOfActiveMemberships();

        var numberOfExpiredMemberships = await _membershipService.GetNumberOfExpiredMemberships();

        var classes=await _bookingService.GetClassesWithHighestNumberOfBookings();

        DashboardSummary dashboardSummary=new(numberOfActiveMemberships, numberOfExpiredMemberships,classes);

        return Result.Success(dashboardSummary); 
    }
}
