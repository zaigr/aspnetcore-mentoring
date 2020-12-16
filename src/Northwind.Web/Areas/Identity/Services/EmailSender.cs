using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Northwind.Web.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Northwind.Web.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;

        private readonly SendGridClient _client;

        public EmailSender(IOptions<SendGridOptions> options)
        {
            _options = options.Value;

            _client = new SendGridClient(_options.ApiKey);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_options.SenderMail, _options.User),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage,
            };

            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(enable: false, enableText: false);

            await _client.SendEmailAsync(msg);
        }
    }
}
