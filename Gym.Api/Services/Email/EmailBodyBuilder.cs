namespace Gym.Api.Services.Email;

public class EmailBodyBuilder : IEmailBodyBuilder
{
    public string GetEmailBody(string template,Dictionary<string,string> placeHolders)
    {
        var filePath = $"{Directory.GetCurrentDirectory()}/templates/{template}.html";

        StreamReader streamReader = new(filePath);

        var templateContent = streamReader.ReadToEnd();

        streamReader.Close();


        foreach(var placeHolder in placeHolders)
        {
            templateContent = templateContent.Replace(placeHolder.Key, placeHolder.Value);
        }

        return templateContent;

    }
}