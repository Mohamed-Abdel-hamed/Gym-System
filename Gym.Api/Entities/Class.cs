namespace Gym.Api.Entities;

public class Class
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }= DateTime.Now;
    public TimeSpan Duration { get; set; }
    public int Capacity { get; set; } = 20;

    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = default!;

    public ICollection<Booking> Bookings { get; set; } = [];
}
