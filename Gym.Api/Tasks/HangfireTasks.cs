using Gym.Api.Abstractions.Consts;
using Gym.Api.Persistence;
using Gym.Api.Services.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Tasks;

public class HangfireTasks(ApplicationDbContext context,
    IEmailBodyBuilder emailBodyBuilder,
    IEmailSender emailSender)
{
    private readonly ApplicationDbContext _context = context;
    private readonly IEmailBodyBuilder _emailBodyBuilder = emailBodyBuilder;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task AlertToExpiresMember()
    {

        var nextWeek = DateTime.Today.AddDays(5);

        var members = _context.Members
            .Include(m => m.User)
            .Include(m => m.Memberships)
             .Where(m => m.Memberships.Any(ms => ms.Status == MembershipStatus.Active))
            .Where(ms => ms.Memberships
            .OrderByDescending(ms => ms.EndDate)
            .Last().EndDate <= nextWeek)
            .ToList();

        foreach (var member in members)
        {
            var expiredDate = member.Memberships
                .Last().EndDate.ToString();
            var placeHolder = new Dictionary<string, string>
        {
            {"{{MemberName}}",member.User.FirstName},
            {"{{ExpiryDate}}",expiredDate! },

        };
            var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.AlertMembership, placeHolder);
            await _emailSender.SendEmailAsync(member.User.Email!, "Gym Memberships Expire", body);
        }
    }

}
