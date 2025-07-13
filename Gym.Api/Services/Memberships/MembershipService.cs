using Azure;
using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using Stripe.V2;
using System.Numerics;

namespace Gym.Api.Services.Memberships;

public class MembershipService(ApplicationDbContext context) : IMembershipService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<string>> SubscribeAsync(int planeId, bool autoRenew,string userId,CancellationToken cancellation=default)
    {
       var member=await _context.Members.SingleOrDefaultAsync(x=>x.UserId==userId,cancellation);

        if(member is null)
            return Result.Failure<string>(UserErrors.UserNotFound);

        var plane = await _context.SubscriptionPlans.FindAsync(planeId,cancellation);

        if(plane is null)
            return Result.Failure<string>(SubscriptionPlanError.NotFound);

        var isExistsMembership = await _context.Memberships.AnyAsync(x => x.MemberId == member.Id && x.PlanId == planeId, cancellation);

        if(isExistsMembership)
            return Result.Failure<string>(MembershipErrors.AlreadyExists);


        bool supportsAutoRenewal = plane.SupportsAutoRenewal && autoRenew;

      var  result = await CreateSubscribe(member.Id, 
          plane.Id, plane.DurationInDays,
          supportsAutoRenewal, plane.Price, 
          plane.Name, cancellation);


        return result.IsSuccess
       ? Result.Success(result.Value)
      : Result.Failure<string>(MembershipErrors.NotCompeleteSubscription);
    }
    public async Task<Result<string>> SuccessAsync(int id)
    {
        var membership = await _context.Memberships.FindAsync(id);

        if (membership is null)
            return Result.Failure<string>(SubscriptionPlanError.NotFound);

        var payment = await _context.Payments.SingleOrDefaultAsync(x=>x.MembershipId==membership.Id);

        if (payment is null)
            return Result.Failure<string>(new Error("payment not found", "payment not found",StatusCodes.Status404NotFound));

        SessionService service = new();
        Session session = service.Get(payment.SessionId);
        payment.PaymentIntentId = session.PaymentIntentId;
        payment.PaymentStatus = session.PaymentStatus;
        membership.AutoRenewPaid = true;
        membership.Status = MembershipStatus.Active;
        await _context.SaveChangesAsync();
        return Result.Success(" success subscribe");
    }
    private async Task<Result<string>>CreateSubscribe(int memberId,int planeId, int planeDurationInDays, bool autoRenew,decimal planePrice,string planeName, CancellationToken cancellation=default)
    {
        int durationDays = planeDurationInDays;
        decimal finalPrice = planePrice;

        if (autoRenew)
        {
            durationDays *= 2;
            finalPrice *= 2;
        }

        var endDate = DateTime.Now.AddDays(durationDays);

        Membership membership = new()
        {
            MemberId = memberId,
            PlanId = planeId,
            StartDate = DateTime.Now,
            EndDate = endDate,
            AutoRenew = autoRenew,
            Status = MembershipStatus.Paused
        };

        await _context.AddAsync(membership, cancellation);

        await _context.SaveChangesAsync(cancellation);

        var domain = "https://localhost:7027";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = domain + $"/api/SubscriptionPlane/{planeId}/Memberships/success?id={membership.Id}",
            CancelUrl = domain + $"/SubscriptionPlane/{planeId}/Memberships/error",
        };
        var sessionLineItemOptions = new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(finalPrice * 100),
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = planeName,
                }
            },
            Quantity = 1
        };
        options.LineItems.Add(sessionLineItemOptions);
        var service = new SessionService();

        Session session = service.Create(options);

        Payment payment = new()
        {
            Amount = (long)sessionLineItemOptions.PriceData.UnitAmount / 100,
            SessionId = session.Id,
            MemberId = memberId,
            MembershipId = membership.Id
        };
        await _context.AddAsync(payment, cancellation);
        await _context.SaveChangesAsync(cancellation);
        return Result.Success(session.Url);
    }
}
