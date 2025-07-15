using Gym.Api.Abstractions;

namespace Gym.Api.Services.Bookings;

public interface IBookingService
{
    Task<Result> Book(string userId, int classId, CancellationToken cancellation = default);
}
