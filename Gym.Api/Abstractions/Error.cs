namespace Gym.Api.Abstractions;

public record Error (string Code,string Description,int? statusCode)
{
    public static readonly Error Non = new(string.Empty, string.Empty,null);
}