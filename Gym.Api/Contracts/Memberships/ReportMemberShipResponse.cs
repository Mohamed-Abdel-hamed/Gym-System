using Gym.Api.Abstractions.Consts;

namespace Gym.Api.Contracts.Memberships;

public record ReportMemberShipResponse
    (
     string StartDate,
     string EndDate,
     bool AutoRenewPaid,
     string Status,
     string Member,
     string Plane,
     int NumberOfFreezes
    );