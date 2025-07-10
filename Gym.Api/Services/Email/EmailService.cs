using Gym.Api.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Gym.Api.Services.Email;

public class EmailService(IOptions<MailSettings> _mailSettings) : IEmailSender
{
    private readonly MailSettings _mailSettings = _mailSettings.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailMessage message = new()
        {
            From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
            Body = htmlMessage,
            Subject = subject,
            IsBodyHtml = true
        };
        message.To.Add(email);
        SmtpClient smtpClient = new(_mailSettings.Host)
        {
            Port = _mailSettings.Port,
            Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
            EnableSsl = true
        };
        await smtpClient.SendMailAsync(message);
        smtpClient.Dispose();
    }
}
