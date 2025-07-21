using Gym.Api.Abstractions;
using Gym.Api.Contracts.Common;
using Gym.Api.Contracts.SubscriptionPlans;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;

namespace Gym.Api.Services.SubscriptionPlans;

public class SubscriptionPlanService(ApplicationDbContext context,IMemoryCache memoryCache) : ISubscriptionPlanService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string _cachePrefix = "AllPlans";
    private static readonly HashSet<string> _subscriptionCacheKeys = new();


    public async Task<Result<PaginatedList<SubscriptionPlanResponse>>> GetAllAsync(RequestFilter filter,CancellationToken cancellation = default)
    {

        var cacheKey = $"{_cachePrefix}_{filter.PageNumber}_{filter.PageSize}_{filter.SearchValue}_{filter.SortColumn}_{filter.SortDirection}";

        _subscriptionCacheKeys.Add(cacheKey);

        var query = _context.SubscriptionPlans
        .AsNoTracking()
        .Where(sp =>

            string.IsNullOrEmpty(filter.SearchValue) ||

            sp.Name.ToLower().Contains(filter.SearchValue.ToLower()));

        if (!query.Any())

            return Result.Failure<PaginatedList<SubscriptionPlanResponse>>(SubscriptionPlanError.NotFound);

        

        if (!string.IsNullOrEmpty(filter.SortColumn))
        {
            query = query.OrderBy($"{filter.SortColumn} {filter.SortDirection}");
        }


        var source=query.ProjectToType<SubscriptionPlanResponse>();

        var data = await PaginatedList<SubscriptionPlanResponse> .CreateASync(source,filter.PageNumber,filter.PageSize,cancellation);

      var subscriptionPlans = await _memoryCache.GetOrCreateAsync(cacheKey,
           cach =>
           {
               cach.SlidingExpiration = TimeSpan.FromMinutes(5);
               return  Task.FromResult(data);
           }
          );

        if (subscriptionPlans is null)
            return Result.Failure<PaginatedList<SubscriptionPlanResponse>>(SubscriptionPlanError.NotFound);


        return Result.Success(subscriptionPlans);
    }
    public async Task<Result> AddAsync(SubscriptionPlanRequest request, CancellationToken cancellation = default)
    {
        var isExistsSubscriptionPlan =await _context.SubscriptionPlans.AnyAsync(x => x.Name == request.Name,cancellation);

        if(isExistsSubscriptionPlan)

            return Result.Failure(SubscriptionPlanError.AlreadyExists);
        SubscriptionPlan subscriptionPlan=request.Adapt<SubscriptionPlan>();

        await _context.SubscriptionPlans.AddAsync(subscriptionPlan,cancellation);
        await _context.SaveChangesAsync(cancellation);

        foreach (var key in _subscriptionCacheKeys)
        {
            _memoryCache.Remove(key);
        }
        _subscriptionCacheKeys.Clear();
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

        foreach (var key in _subscriptionCacheKeys)
        {
            _memoryCache.Remove(key);
        }
        _subscriptionCacheKeys.Clear();

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

        foreach (var key in _subscriptionCacheKeys)
        {
            _memoryCache.Remove(key);
        }
        _subscriptionCacheKeys.Clear();

        return Result.Success();
    }
}
