using Gym.Api.Abstractions;
using Gym.Api.Contracts.SubscriptionPlans;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.SubscriptionPlans;

public class SubscriptionPlanService(ApplicationDbContext context) : ISubscriptionPlanService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetAllAsync(CancellationToken cancellation = default)
    {
       var subscriptionPlans = await _context.SubscriptionPlans
            .AsNoTracking()
            .ProjectToType<SubscriptionPlanResponse>()
            .ToListAsync(cancellation);

        if (subscriptionPlans.Count == 0)

            return Result.Failure<IEnumerable<SubscriptionPlanResponse>>(SubscriptionPlanError.NotFound);
        return Result.Success<IEnumerable<SubscriptionPlanResponse>>(subscriptionPlans);
    }
    public async Task<Result> AddAsync(SubscriptionPlanRequest request, CancellationToken cancellation = default)
    {
        var isExistsSubscriptionPlan =await _context.SubscriptionPlans.AnyAsync(x => x.Name == request.Name,cancellation);

        if(isExistsSubscriptionPlan)

            return Result.Failure(SubscriptionPlanError.AlreadyExists);
        SubscriptionPlan subscriptionPlan=request.Adapt<SubscriptionPlan>();

        return Result.Success();
    }
}
