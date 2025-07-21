using Gym.Api.Abstractions;
using Gym.Api.Contracts.Common;
using Gym.Api.Contracts.SubscriptionPlans;

namespace Gym.Api.Services.SubscriptionPlans;

public interface ISubscriptionPlanService
{
    Task<Result<PaginatedList<SubscriptionPlanResponse>>> GetAllAsync(RequestFilter filter,CancellationToken cancellation=default);
   // Task<Result<PaginatedList<SubscriptionPlanResponse>>> GetAll(RequestFilter filter,CancellationToken cancellation =default);
    Task<Result> AddAsync(SubscriptionPlanRequest request,CancellationToken cancellation =default);
    Task<Result> UpdateAsync(int id, SubscriptionPlanRequest request, CancellationToken cancellation = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellation = default);

}
