using DocumentFormat.OpenXml.Spreadsheet;
using Gym.Api;
using Gym.Api.Entities;
using Gym.Api.Persistence;
using Gym.Api.Seeds;
using Gym.Api.Services.Email;
using Gym.Api.Tasks;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;
using Stripe;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard("/jobs",new DashboardOptions
{
    Authorization = [
        new HangfireCustomBasicAuthenticationFilter
        {
            User=app.Configuration.GetValue<string>("HangfireSettings:Username"),
            Pass=app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
        ],
      DashboardTitle = "Gym Management Jobs"
});
StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();


app.UseCors("AllowAll");
app.UseAuthorization();

var scopeFactory=app.Services.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateScope();

var roleManger=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


await DefaultRoles.SeedAsync(roleManger);
await DefaultUser.SeedAdminUserAsync(userManger);




app.MapControllers();
app.UseExceptionHandler();
app.UseRateLimiter();
app.MapHealthChecks("health",new HealthCheckOptions
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});

var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

var emailBuilder= scope.ServiceProvider.GetRequiredService<IEmailBodyBuilder>();

var emailSender= scope.ServiceProvider.GetRequiredService<IEmailSender>();

var tasks = new HangfireTasks(dbContext, emailBuilder, emailSender);

RecurringJob.AddOrUpdate(() => tasks.AlertToExpiresMember(), Cron.Yearly);

app.Run();
