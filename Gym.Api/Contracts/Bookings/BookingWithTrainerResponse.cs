using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Members;
using Gym.Api.Contracts.Users;

namespace Gym.Api.Contracts.Bookings;

public record BookingWithTrainerResponse
(
     MemberSummary Member,
    BookingStatus Status,
    DateTime CreatedDate
);
