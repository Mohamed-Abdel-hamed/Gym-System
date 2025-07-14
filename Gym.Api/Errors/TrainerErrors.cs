using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class TrainerErrors
{
    public static Error NotFound => new("trainer.NotFound", "Not found any trainer", StatusCodes.Status404NotFound);
}
