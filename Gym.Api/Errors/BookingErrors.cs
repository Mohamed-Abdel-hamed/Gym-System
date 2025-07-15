using Gym.Api.Abstractions;

namespace Gym.Api.Errors;

public class BookingErrors
{
    public static Error AlreadyExists => new("Booking.AlreadyExists", "Booking already exists", StatusCodes.Status409Conflict);
}
