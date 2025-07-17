using Gym.Api.Contracts.Bookings;
using Gym.Api.Contracts.Users;

namespace Gym.Api.Contracts.Members;

public record MemberResponse
    (
      IEnumerable<BookingResponse> Bookings,
      UserProfileResponse User
    );
