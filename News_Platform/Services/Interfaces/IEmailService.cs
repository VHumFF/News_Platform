namespace News_Platform.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string templateName, Dictionary<string, string> placeholders);
    }
}
