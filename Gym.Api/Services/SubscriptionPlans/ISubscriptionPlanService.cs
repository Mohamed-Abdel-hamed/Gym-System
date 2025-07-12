using Gym.Api.Abstractions;
using Gym.Api.Contracts.SubscriptionPlans;

namespace Gym.Api.Services.SubscriptionPlans;

public interface ISubscriptionPlanService
{
    Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetAllAsync(CancellationToken cancellation=default);
    Task<Result> AddAsync(SubscriptionPlanRequest request,CancellationToken cancellation =default);

}
