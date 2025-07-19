namespace Gym.Api.Contracts.Classes;

public record DashboardClassBookingResponse
    (
     int Id,
    string Title,
    DateTime StartDate,
    int Capacity,
    double BookingsAverage
    );