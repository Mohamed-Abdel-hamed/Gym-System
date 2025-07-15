namespace Gym.Api.Entities;

public class Class
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public TimeSpan Duration { get; set; }
    public int Capacity { get; set; }

    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = default!;

    public ICollection<Booking> Bookings { get; set; } = [];
}
