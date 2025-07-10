namespace Gym.Api.Entities;

public class Staff
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
    public DateOnly HireDate { get; set; }
}
