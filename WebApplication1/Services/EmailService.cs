using System.Net;
using System.Net.Mail;

namespace WebApplication1
{
    public class EmailService
    {
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            MailAddress emailFrom = new("webweda.system@outlook.com", "Web Weda");
            MailAddress emailTo = new(to);
            MailMessage m = new(emailFrom, emailTo);
            m.Subject = subject;
            m.Body = $"<p>{message}</p>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new("smtp.office365.com", 587);
            smtp.Credentials = new NetworkCredential("webweda.system@outlook.com", "Warcraft3!");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}