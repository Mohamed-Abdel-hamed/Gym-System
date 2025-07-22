using Gym.Api.Abstractions;
using Gym.Api.Contracts.Dashboards;

namespace Gym.Api.Services.Memberships;

public interface IMembershipService
{
    Task<Result<string>> SubscribeAsync(int planeId,bool autoRenew,string userId, CancellationToken cancellation = default);
    Task<Result<string>> SuccessAsync(int id);
    Task<Result<IEnumerable<ChartItemResponse>>> MembershipsPerDay(DateTime? startDate, DateTime? endDate);
    Task<int> GetNumberOfActiveMemberships();
    Task<int> GetNumberOfExpiredMemberships();
}
