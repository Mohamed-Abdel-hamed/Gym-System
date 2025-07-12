namespace Gym.Api.Entities;

public class Payment
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;

    public int MembershipId { get; set; }
    public Membership Membership { get; set; } = default!;

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;

    // Stripe-related tracking
    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? PaymentStatus { get; set; }
}
