namespace Gym.Api.Entities;

public class Trainer
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
    public DateOnly HireDate { get; set; }
}
