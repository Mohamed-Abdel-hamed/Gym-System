namespace Gym.Api.Entities;

public class Member
{
    public int Id { get; set; }
    public string UserId { get; set; }=string.Empty;
    public ApplicationUser User { get; set; } = default!;
    public ICollection<Membership> Memberships { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}
 