using Gym.Api.Abstractions;
using Gym.Api.Contracts.SubscriptionPlans;
using Gym.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gym.Api.Services.SubscriptionPlans;

public class SubscriptionPlanService(ApplicationDbContext context) : ISubscriptionPlanService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetAllAsync(CancellationToken cancellation = default)
    {
       var subscriptionPlans = await _context.SubscriptionPlans
            .AsNoTracking()
            .Select(x=>new SubscriptionPlanResponse
            (
                x.Name,
                x.UnlimitedDailyEntries,
                x.MaxClassBookingsPerDay,
                x.MaxClassBookingsInFuture,
                x.MaxFreezesPerYear,
                x.MaxFreezeDays
            )
            )
            .ToListAsync(cancellation);
        if(subscriptionPlans.Count==0)

            return Result.Failure<IEnumerable<SubscriptionPlanResponse >> (new Error("SubscriptionPlans.NotFound", "Not found any SubscriptionPlans"));
        return Result.Success<IEnumerable<SubscriptionPlanResponse>>(subscriptionPlans);
    }
}
