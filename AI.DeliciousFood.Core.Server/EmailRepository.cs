using AI.DeliciousFood.Core.Common.ClientHelper;
using System.Net;
using System.Net.Mail;

namespace AI.DeliciousFood.Core.Server;

public interface IEmailRepository
{
    void SendEmail(string email, string subject, string body);
}

public class EmailRepository(EmailConfigHelper emailConfig) : IEmailRepository
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(emailConfig.SmtpFrom);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient(emailConfig.SmtpHost, emailConfig.SmtpPort);
        smtp.Credentials = new NetworkCredential(emailConfig.SmtpUserName, emailConfig.FromPassword);
        smtp.EnableSsl = emailConfig.FromEnableSsl;

        smtp.Send(mail);
    }
}