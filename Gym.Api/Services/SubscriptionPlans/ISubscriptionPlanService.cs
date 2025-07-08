using Gym.Api.Contracts.SubscriptionPlans;

namespace Gym.Api.Services.SubscriptionPlans;

public interface ISubscriptionPlanService
{
    Task<IEnumerable<SubscriptionPlanResponse>> GetAllAsync(CancellationToken cancellation=default);
}
