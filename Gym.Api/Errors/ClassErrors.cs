using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class ClassErrors
{
    public static Error NotFound => new("classes.NotFound", "Not found any clesses", StatusCodes.Status404NotFound);
    public static Error AlreadyExists => new("class.AlreadyExists", "class plan already exists", StatusCodes.Status409Conflict);

}
