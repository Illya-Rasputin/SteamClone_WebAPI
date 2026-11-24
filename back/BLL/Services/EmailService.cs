using BLL.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BLL.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> options,
            ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<ServiceResponse> SendConfirmationEmailAsync(
            string email,
            string confirmationUrl)
        {
            if (string.IsNullOrWhiteSpace(_settings.Host)
                || _settings.Port is < 1 or > 65535
                || !MailAddress.TryCreate(_settings.FromEmail, out var fromAddress))
            {
                return ServiceResponse.Error(
                    "Email delivery is not configured. Configure the SMTP EmailSettings before registering users.");
            }

            using var message = new MailMessage
            {
                From = new MailAddress(fromAddress.Address, _settings.FromName),
                Subject = "Confirm your SteamClone account",
                Body = $"Confirm your email address by opening this link:\n\n{confirmationUrl}",
                IsBodyHtml = false
            };
            message.To.Add(email);

            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl
            };

            if (!string.IsNullOrWhiteSpace(_settings.UserName))
            {
                client.Credentials = new NetworkCredential(
                    _settings.UserName,
                    _settings.Password);
            }

            try
            {
                await client.SendMailAsync(message);
                return ServiceResponse.Success("Confirmation email sent");
            }
            catch (SmtpException exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to send an email confirmation message to {Email}",
                    email);
                return ServiceResponse.Error(
                    "The account could not be registered because the confirmation email could not be sent.");
            }
        }
    }
}
