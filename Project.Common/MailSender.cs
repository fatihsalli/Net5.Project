using System.Net;
using System.Net.Mail;

namespace Project.Common
{
    public static class MailSender
    {
        public static void SendEmail(string email, string subject, string message)
        {
            MailMessage sender = new MailMessage();
            sender.From = new MailAddress("fatih.sallidemo@outlook.com", "YZL3159");
            sender.To.Add(email);
            sender.Subject = subject;
            sender.Body = message;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential("fatih.sallidemo@outlook.com", "Sez123456--");
            smtpClient.Port = 587;
            smtpClient.Host = "smtp-mail.outlook.com";
            smtpClient.EnableSsl = true;

            smtpClient.Send(sender);
        }
    }
}
