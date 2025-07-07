using Gym.Api.Abstractions.Consts;
using Microsoft.AspNetCore.Identity;

namespace Gym.Api.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        if(!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Admin });
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Staff }); 
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Trainer });
            await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Member});
        }
    }
}
