using System.Threading.Tasks;
using Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MimeKit;

namespace Infrastructure.Emails
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public EmailService(IEmailConfiguration emailConfiguration, IWebHostEnvironment webHostEnvironment)
        {
            _emailConfiguration = emailConfiguration;
            _webHostEnviroment = webHostEnvironment;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = CreateMessage(email, subject, htmlMessage);

            await SendMessageAsync(message);
        }

        private MimeMessage CreateMessage(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.FromMail));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            return message;
        }

        private async Task SendMessageAsync(MimeMessage message)
        {
            if (_webHostEnviroment.IsDevelopment())
            {
                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync
                        (_emailConfiguration.SmtpHost, _emailConfiguration.SmtpPort, SecureSocketOptions.None);
                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                }
            }
            else
            {
                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync
                        (_emailConfiguration.SmtpHost, _emailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
                    await smtpClient.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                }
            }
        }
    }
}