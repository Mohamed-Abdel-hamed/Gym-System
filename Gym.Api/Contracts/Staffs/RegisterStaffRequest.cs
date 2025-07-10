using Gym.Api.Contracts.Authentications;

namespace Gym.Api.Contracts.Staffs;

public record RegisterStaffRequest
    (
    RegisterRequest Info,
   DateOnly HireDate
    );
