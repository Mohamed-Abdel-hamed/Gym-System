using Gym.Api.Abstractions.Consts;

namespace Gym.Api.Entities;

public class Membership
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;

    public int PlanId { get; set; }
    public SubscriptionPlan Plan { get; set; } = default!;

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public MembershipStatus Status { get; set; } = MembershipStatus.Active;

    // Auto-renewal options (per user instance)
    public bool AutoRenew { get; set; } = false;
    public bool AutoRenewPaid { get; set; } = false;
    public ICollection<Payment> Payments { get; set; } = [];
}
