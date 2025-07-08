namespace Gym.Api.Abstractions;

public record Error (string Code,string Description)
{
    public static readonly Error Non = new(string.Empty, string.Empty);
}