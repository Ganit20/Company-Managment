using Company_Managment.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Company_Managment.Controllers
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<Confirmation> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public Confirmation Options { get; }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendEmailKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("bartlomiej.sulek10@gmail.com", Options.SendEmailUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message,
            };
            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);

        }
    }
}
