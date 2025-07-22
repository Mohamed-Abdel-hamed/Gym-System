using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Gym.Api.Services.Auth;
using Gym.Api.Services.SubscriptionPlans;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Gym.Api.Authentications;
using FluentValidation.AspNetCore;
using FluentValidation;
using Gym.Api.Settings;
using Gym.Api.Services.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Gym.Api.Services.Memberships;
using Gym.Api.Services.MembershipFreezes;
using Gym.Api.Services.Classes;
using Gym.Api.Services.Bookings;
using Hangfire;
using Gym.Api.Services.Members;
using Gym.Api.Services.Trainers;
using Gym.Api.Services.Users;
using Gym.Api.Services.Reports;
using Gym.Api.Services.Roles;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.DataProtection;

namespace Gym.Api;

public static class ConfigurationService
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextConfig(configuration)
            .AddIdentityConfig(configuration)
            .AddServicesConfig()
            .AddMapsterConfig()
            .AddFluentValidationConfig()
            .AddHangfireConfig(configuration)
            .RateLimitingConfig()
            .AddSwagerConfig();
        services.AddCors(optoins =>
        {
            optoins.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.WithOrigins("http://localhost:500");
            });
        });

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        services.Configure<StripeSettings>(configuration.GetSection("stripe"));

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        services.AddHealthChecks()
           .AddDbContextCheck<ApplicationDbContext>("database");

        services.AddMemoryCache();

        services.AddDataProtection().SetApplicationName(nameof(Gym));

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
    private static IServiceCollection AddServicesConfig(this IServiceCollection services)
    {
        services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IEmailBodyBuilder, EmailBodyBuilder>();
        services.AddScoped<IMembershipService,MembershipService>();
        services.AddScoped<IMembershipFreezeService, MembershipFreezeService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IBookingService,BookingService>();
        services.AddScoped<IMemberSerive, MemberSerive>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IReportService,ReportService>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser,IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

        services.AddScoped<IJwtProvider, JwtProvider>();

        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"]
                };
            });

        return services;
    }
    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
    private static IServiceCollection AddSwagerConfig(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
    private static IServiceCollection AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();
        return services;
    }
    private static IServiceCollection RateLimitingConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.AddTokenBucketLimiter("token", options =>
            {
                options.TokenLimit = 2;
                options.QueueLimit = 1;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                options.TokensPerPeriod = 2;
                options.AutoReplenishment = true;
            });
        });
            

        return services;
    }
}
