using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Classes;

namespace Gym.Api.Contracts.Bookings;

public record BookingResponse
    (
    string Status,
    DateTime CreatedDate,
    ClassResponse Class
    );