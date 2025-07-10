using Gym.Api.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Gym.Api.Services.Email;

public class EmailService(IOptions<MailSettings> _mailSettings,IWebHostEnvironment webHostEnvironment) : IEmailSender
{
    private readonly MailSettings _mailSettings = _mailSettings.Value;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailMessage message = new()
        {
            From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
            Body = htmlMessage,
            Subject = subject,
            IsBodyHtml = true
        };
        message.To.Add(_webHostEnvironment.IsDevelopment() ? "mohamed7abdelhamid594@gmail.com" : email);
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
