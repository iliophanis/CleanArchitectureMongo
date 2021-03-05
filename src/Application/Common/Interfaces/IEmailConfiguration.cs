namespace Application.Common.Interfaces
{
    public interface IEmailConfiguration
    {
        string SmtpHost { get; set; }
        int SmtpPort { get; set; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
        string FromMail { get; set; }
        string FromName { get; set; }
    }
}