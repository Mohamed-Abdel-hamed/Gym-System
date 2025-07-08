using Gym.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gym.Api.Services.Auth;

public class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
}
