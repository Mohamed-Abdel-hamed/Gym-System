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

        await _context.SubscriptionPlans.AddAsync(subscriptionPlan,cancellation);
        await _context.SaveChangesAsync(cancellation);

        return Result.Success();
    }
    public async Task<Result> UpdateAsync(int id,SubscriptionPlanRequest request, CancellationToken cancellation = default)
    {
        var isExistsSubscriptionPlan = await _context.SubscriptionPlans.AnyAsync(x => x.Name == request.Name&&x.Id!=id, cancellation);

        if (isExistsSubscriptionPlan)

            return Result.Failure(SubscriptionPlanError.AlreadyExists);

        var subscriptionPlan = await _context.SubscriptionPlans
       .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (subscriptionPlan is null)
            return Result.Failure(SubscriptionPlanError.NotFound);

        subscriptionPlan=request.Adapt(subscriptionPlan);


        await _context.SaveChangesAsync(cancellation);

        return Result.Success();
    }
    public async Task<Result> DeleteAsync(int id, CancellationToken cancellation = default)
    {
        var subscriptionPlan = await _context.SubscriptionPlans
       .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (subscriptionPlan is null)
            return Result.Failure(SubscriptionPlanError.NotFound);

       _context.SubscriptionPlans.Remove(subscriptionPlan);


        await _context.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}
