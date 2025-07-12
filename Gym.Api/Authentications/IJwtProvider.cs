using Gym.Api.Entities;

namespace Gym.Api.Authentications;

public interface IJwtProvider
{
   public  Task<(string token, int ExpiresIn)> GenerateToken(ApplicationUser user);
}
