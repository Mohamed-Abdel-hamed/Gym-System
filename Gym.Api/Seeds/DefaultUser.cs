using Gym.Api.Consts;
using Gym.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gym.Api.Seeds;

public static class DefaultUser
{
    public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
            ApplicationUser admin = new()
            {
                UserName = "admin",
                Email = "admin@bookify.com",
                FirstName = "Admin",
                LastName="admin",
                EmailConfirmed = true
            };
        var user= await userManager.FindByIdAsync(admin.Id);
        if(user is null)
        {
            await userManager.CreateAsync(admin, "P@ssword123");
            await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }
    }
}
 