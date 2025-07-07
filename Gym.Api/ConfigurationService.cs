using Gym.Api.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api;

public static class ConfigurationService
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextConfig(configuration)
            .AddIdentityConfig();
        return services;
    }
    public static IServiceCollection AddDbContextConfig(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString=configuration.GetConnectionString("DefaultConnection")??

            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>

        options.UseSqlServer(connectionString));
        return services;
    }
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        });
        return services;
    }
}
