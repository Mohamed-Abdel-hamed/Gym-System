using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class ClassErrors
{
    public static Error NotFound => new("classes.NotFound", "Not found any clesses", StatusCodes.Status404NotFound);
    public static Error AlreadyExists => new("class.AlreadyExists", "class plan already exists", StatusCodes.Status409Conflict);
    public static Error CompleteClass => new("Class.CompleteClass", "can not booking because class is comleted", StatusCodes.Status400BadRequest);
    public static Error AlreadyStarted => new("Class.AlreadyStarted", "can not booking because class already started", StatusCodes.Status400BadRequest);

}
