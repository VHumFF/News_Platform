using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using News_Platform.Models;
using News_Platform.Repositories.Interfaces;
using News_Platform.Services.Interfaces;


namespace News_Platform.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private Dictionary<string, EmailTemplate> _emailTemplateCache;

        public EmailService(IConfiguration configuration, IEmailTemplateRepository emailTemplateRepository)
        {
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
            _emailTemplateCache = new Dictionary<string, EmailTemplate>();

            Task.Run(LoadEmailTemplatesAsync).Wait();
        }

        private async Task LoadEmailTemplatesAsync()
        {
            _emailTemplateCache = await _emailTemplateRepository.GetAllTemplatesAsync();
        }

        public async Task SendEmailAsync(string toEmail, string templateName, Dictionary<string, string> placeholders)
        {
            if (!_emailTemplateCache.ContainsKey(templateName))
                throw new Exception($"Email template '{templateName}' not found in cache.");

            var template = _emailTemplateCache[templateName].Template;

            // Replace placeholders with actual values
            foreach (var placeholder in placeholders)
            {
                template = template.Replace(placeholder.Key, placeholder.Value);
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("News", _configuration["SmtpSettings:Username"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = _emailTemplateCache[templateName].Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = template };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(
                    _configuration["SmtpSettings:Host"],
                    int.Parse(_configuration["SmtpSettings:Port"]),
                    SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(
                    _configuration["SmtpSettings:Username"],
                    _configuration["SmtpSettings:Password"]);

                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
