using Gym.Api.Contracts.Classes;

namespace Gym.Api.Contracts.Dashboards;

public record DashboardSummary
    (
     int NumberOfActiveMemberships,
     int NumberOfExpiredMemberships,
     IEnumerable<DashboardClassBookingResponse> Classes
    );
