namespace Gym.Api.Services.Email;
    public interface IEmailBodyBuilder
    {
        string GetEmailBody(string template, Dictionary<string, string> placeHolders);
    }
