using Gym.Api.Contracts.Bookings;

namespace Gym.Api.Contracts.Classes;

public record ClassWithTrainerResponse
    (
     string Title,
    DateTime StartDate,
    TimeSpan Duration,
    int Capacity,
    IEnumerable<BookingWithTrainerResponse> Bookings
    );
