using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Classes;

namespace Gym.Api.Contracts.Bookings;

public record BookingResponse
    (
    BookingStatus Status,
    DateTime CreatedDate,
    ClassResponse Class
    );