using Gym.Api.Contracts.Users;

namespace Gym.Api.Contracts.Members;

public record MemberSummary
    (
    UserProfileResponse User
    );