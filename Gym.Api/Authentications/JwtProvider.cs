using Gym.Api.Abstractions.Consts;
using Gym.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gym.Api.Authentications;

public class JwtProvider(IOptions<JwtOptions> options,UserManager<ApplicationUser> userManager) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<(string token, int ExpiresIn)> GenerateToken(ApplicationUser user)
    {
        
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Email, user.Email!),
        new(JwtRegisteredClaimNames.GivenName, user.FirstName),
        new(JwtRegisteredClaimNames.FamilyName, user.LastName),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); 
        }



        var symmtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(symmtricSecurityKey,SecurityAlgorithms.HmacSha256);
        var expiresIn = 30;
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: signingCredentials
            );
        return (token:new JwtSecurityTokenHandler().WriteToken(token),expiresIn*60);
    }
}
