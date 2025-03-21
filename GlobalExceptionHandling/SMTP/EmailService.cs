using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Middleware.SMTP
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendResetPasswordEmailAsync(string email, string token)
        {
            var fromEmail = _configuration["SMTP:SenderEmail"];
            var fromPassword = _configuration["SMTP:Password"];
            var smtpHost = _configuration["SMTP:Host"];
            var smtpPort = _configuration["SMTP:Port"];
            var enableSSL = Convert.ToBoolean(_configuration["SMTP:EnableSSL"]);
            string resetPasswordUrl = _configuration["App:ResetPasswordUrl"]; // Get the URL from appsettings.json

            // Log SMTP Configuration
            _logger.LogInformation($"SMTP Email: {fromEmail}");
            _logger.LogInformation($"SMTP Host: {smtpHost}");
            _logger.LogInformation($"SMTP Port: {smtpPort}");
            _logger.LogInformation($"SMTP SSL Enabled: {enableSSL}");
            _logger.LogInformation($"Reset Password URL: {resetPasswordUrl}");

            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                _logger.LogError("SMTP sender email is not configured.");
                throw new Exception("SMTP sender email is missing in configuration.");
            }

            try
            {
                var fromAddress = new MailAddress(fromEmail, "Greeting APP");
                var toAddress = new MailAddress(email);

                using var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = int.Parse(smtpPort ?? "587"), // Default to 587 if null
                    EnableSsl = enableSSL,
                    Credentials = new NetworkCredential(fromEmail, fromPassword)
                };

                string resetLink = $"{resetPasswordUrl}?token={token}";// Use the configured reset link
                _logger.LogInformation($"Generated Reset Link: {resetLink}");

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Reset Your Password",
                    Body = $"Click the link to reset your password: <a href='{resetLink}'>{resetLink}</a>",
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
                throw new Exception("Email sending failed", ex);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromEmail = _configuration["SMTP:SenderEmail"];
            var fromPassword = _configuration["SMTP:Password"];
            var smtpHost = _configuration["SMTP:Host"];
            var smtpPort = _configuration["SMTP:Port"];

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Greeting APP", fromEmail));
                emailMessage.To.Add(new MailboxAddress("Recipient", to));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(smtpHost, int.TryParse(smtpPort, out int port) ? port : 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(fromEmail, fromPassword);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);

                Console.WriteLine($"[x] Email successfully sent to {to}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Email sending failed: {ex.Message}");
                throw;
            }
        }

    }
}
