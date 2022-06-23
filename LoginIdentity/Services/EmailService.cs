using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LoginIdentity.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
    public class EmailService : IEmailService
    {
		private readonly MailSettings _settings;

        public EmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail,string subject, string htmlMessage)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(_settings.DefaultFrom, _settings.DisplayName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                
            };
            mail.To.Add(new MailAddress(toEmail));

            var client = new SmtpClient()
            {
                Port = _settings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = _settings.SmtpServer,
                EnableSsl = _settings.EnableSsl,
                Credentials = !string.IsNullOrWhiteSpace(_settings.DefaultFrom)
                    ? new NetworkCredential(_settings.DefaultFrom, _settings.Password)
                    : null
            };
            await client.SendMailAsync(mail);
        }

    }
	public class MailSettings
	{
		public bool EnableSsl { get; set; }
		public string SmtpServer { get; set; }
		public int Port { get; set; }
		public string DefaultFrom { get; set; }
		public string DisplayName { get; set; }
        public string Password { get; set; }
    }
}
