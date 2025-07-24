using System;
using System.Net;
using System.Net.Mail;

namespace CodeEditor
{
    public class MailService
    {
        public void SendMail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Change this to your SMTP host
                    Port = 587,             // Change port if needed
                    Credentials = new NetworkCredential("youremail@example.com", "yourpassword"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("youremail@example.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle errors
                throw new Exception("Mail sending failed: " + ex.Message);
            }
        }
    }

}