using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Gym.Api.Services.MembershipFreezes;

public class MembershipFreezeService(ApplicationDbContext context) : IMembershipFreezeService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(string userId, int membershipId,CancellationToken cancellation=default)
    {
        var member = await _context.Members.SingleOrDefaultAsync(x => x.UserId == userId, cancellation);

        if (member is null)
            return Result.Failure(UserErrors.UserNotFound);

        /////////////////////////////////////////////////////////////

        var membership = await _context.Memberships
            .Include(x=>x.Plan)
            .Include(x=>x.Freezes)
            .SingleOrDefaultAsync(x=>x.MemberId==member.Id&&x.Id==membershipId,cancellation);

        if (membership is null) 
            return Result.Failure(MembershipErrors.NotFound);

        ///////////////////////////////////////////////////////////////////////////////////////////

        if (membership.Status!=MembershipStatus.Active)
            return Result.Failure(MembershipErrors.NotActive);

        ///////////////////////////////////////////////////////////////////////////////////////////
        
        if (membership.Plan.MaxFreezeDays == 0)
            return Result.Failure(MembershipFreezeErrors.NotExistsFreeze);

        ///////////////////////////////////////////////////////////////////////////////////////////

        var now = DateTime.Now;

        var remainingSubscriptionPeriod = membership.EndDate!.Value- now;

        double TotalDays = remainingSubscriptionPeriod.Days;


        if (TotalDays < membership.Plan.MaxFreezeDays)
            return Result.Failure(MembershipFreezeErrors.NotAllowedFreeze);

        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
        // If the member paid for 2 periods (AutoRenewPaid == true),
        // we only allow freezing in the first half of the duration.
        // So we divide total days by 2 and make sure it's still at least 365 days.

        var membershipTotalDays = (membership.EndDate.Value - membership.StartDate).Days;


        membershipTotalDays = membership.AutoRenewPaid
     ? membershipTotalDays / 2
     : membershipTotalDays;

        // If paid for 2 periods, we only allow freezing in the first one (half duration)
        if (membershipTotalDays < 365)
            return Result.Failure(MembershipErrors.LimitDuration);

        ///////////////////////////////////////////////////////////////////////////////////////////
        ///

        var currentFreeze = membership.Freezes
             .Where(f => f.EndDate >= membership.StartDate && f.StartDate <= membership.EndDate&&f.EndDate>now);

        var usedFreezeDays= currentFreeze.Sum(f=>(f.EndDate-f.StartDate).Days);
        var maxAllowedFreezeDays = membership.Plan.MaxFreezeDays * membership.Plan.MaxFreezesPerYear;
        var remainingFreezeDays = maxAllowedFreezeDays - usedFreezeDays;

        if (remainingFreezeDays <= 0 || currentFreeze.Count() >= membership.Plan.MaxFreezesPerYear)
            return Result.Failure(MembershipFreezeErrors.NotAllowedFreezeCount);

        // Use the smaller of MaxFreezeDays and the remaining allowed days
        double allowedFreezeDays = Math.Min(membership.Plan.MaxFreezeDays, remainingFreezeDays);

        MembershipFreeze freeze = new()
        {
            StartDate = now,
            EndDate = now.AddDays(allowedFreezeDays),
            MembershipId = membership.Id,
        };

        membership.Status = MembershipStatus.Freeze;

        await _context.MembershipFreezes.AddAsync(freeze, cancellation);
        await _context.SaveChangesAsync(cancellation);


        return Result.Success();
    }

    public async Task<Result> UnFreezeAsync(string userId,int membershipId,CancellationToken cancellation=default)
    {
        var member = await _context.Members.SingleOrDefaultAsync(x => x.UserId == userId, cancellation);

        if (member is null)
            return Result.Failure(UserErrors.UserNotFound);
        /////////////////////////////////////////////////////////////

        var membership = await _context.Memberships
            .Include(m=>m.Freezes)
            .SingleOrDefaultAsync(x => x.MemberId == member.Id && x.Id == membershipId, cancellation);
        ///////////////////////////////////////////////////////////////////////////////////////////

        if (membership is null)
            return Result.Failure(MembershipErrors.NotFound);
        ///////////////////////////////////////////////////////////////////////////////////////////

        if (membership.Status != MembershipStatus.Freeze)
            return Result.Failure(MembershipErrors.NotFreeze);

        var now = DateTime.Now;

        // var tomorrow = DateTime.Now.Date.AddDays(1);
        var currentFreeze = membership.Freezes
             .Where(f => f.EndDate >= membership.StartDate &&
             f.StartDate <= membership.EndDate &&
             f.EndDate > now).SingleOrDefault();

        if (currentFreeze is null)
            return Result.Failure(MembershipFreezeErrors.NotExistsFreeze);

        membership.Status = MembershipStatus.Active;

        currentFreeze.EndDate = now;

        await _context.SaveChangesAsync(cancellation);
        return Result.Success();
    }
    public async Task ReactivateFrozenMemberships()
    {
        var memberships =await _context.Memberships
            .Include(ms=>ms.Freezes)
            .Where(ms=>ms.Status==MembershipStatus.Freeze)
            .Where(ms=>ms.Freezes.
            OrderByDescending(ms=>ms.EndDate)
            .Last().EndDate<=DateTime.Today)
            .ToListAsync();

        //foreach ( var membership in memberships)
        //{
        //    Console.WriteLine($"{membership.Id} - {membership.Freezes.First().Id} - {membership.Freezes.First().EndDate}");
        //}

        memberships.ForEach(ms =>
        {
            ms.Status = MembershipStatus.Active;
        });

       await _context.SaveChangesAsync();
    }
}
