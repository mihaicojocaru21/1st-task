using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HelloBlazor.Data
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailFrom = _config["Email:From"] ?? throw new InvalidOperationException("Email:From not configured");
            var smtpHost = _config["Email:Smtp:Host"] ?? throw new InvalidOperationException("Email:Smtp:Host not configured");
            var smtpUser = _config["Email:Smtp:User"] ?? throw new InvalidOperationException("Email:Smtp:User not configured");
            var smtpPass = _config["Email:Smtp:Password"] ?? throw new InvalidOperationException("Email:Smtp:Password not configured");

            // Parse port safely
            int smtpPort = int.TryParse(_config["Email:Smtp:Port"], out var parsedPort)
                ? parsedPort
                : 587; // default fallback

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HelloBlazor App", emailFrom));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUser, smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}