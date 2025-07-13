using Gym.Api.Abstractions.Consts;

namespace Gym.Api.Entities;

public class Booking
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;

    public int ClassId { get; set; }
    public Class Class { get; set; } = default!;

    public BookingStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int? WaitlistPosition { get; set; }
}
