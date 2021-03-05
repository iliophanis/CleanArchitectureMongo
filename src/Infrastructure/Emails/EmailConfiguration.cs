using Application.Common.Interfaces;

namespace Infrastructure.Emails
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromMail { get; set; }
        public string FromName { get; set; }
    }
}