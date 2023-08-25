using BitNews.Helpers;
using BitNews.Services.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace BitNews.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();

            string fromAddress = !string.IsNullOrEmpty(from) ? from : _emailSettings.FromAddress;

            if (!string.IsNullOrEmpty(fromAddress))
            {
                email.From.Add(MailboxAddress.Parse(fromAddress));
            }

            if (!string.IsNullOrEmpty(to))
            {
                email.To.Add(MailboxAddress.Parse(to));
            }

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Username, _emailSettings.Password);
            smtp.Send(email);
        }
    }
}
