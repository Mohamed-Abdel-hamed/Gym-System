using Gym.Api.Abstractions;
using Gym.Api.Contracts.Classes;

namespace Gym.Api.Services.Bookings;

public interface IBookingService
{
    Task<Result> Book(string userId, int classId, CancellationToken cancellation = default);
    Task<IEnumerable<DashboardClassBookingResponse>> GetClassesWithHighestNumberOfBookings();
}
