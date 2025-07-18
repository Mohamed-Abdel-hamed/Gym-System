namespace Gym.Api.Contracts.Dashboards;

public record DashboardSummary
    (
     int NumberOfActiveMemberships,
     int NumberOfExpiredMemberships
    );
