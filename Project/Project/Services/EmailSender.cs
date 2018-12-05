using System;
using System.Net;
using System.Net.Mail;

namespace Project.Services
{
    public class EmailSender
    {
        // Borrow dummy email from old project
        private static readonly NetworkCredential Credentials =
            new NetworkCredential("leitbugs.test@gmail.com", "leitbugs123");

        private static readonly MailAddress SenderEmail =
            new MailAddress("leitbugs.test@gmail.com", "Project E Testing Email");

        private const string SmtpServerAddress = "smtp.gmail.com";
        private const int SmtpServerPort = 587;

        public static void SendEmail(string email, string subject, string message)
        {
            var mailMessage = new MailMessage(SenderEmail, new MailAddress(email))
            {
                Body = message,
                Subject = subject
            };

            using (var client = new SmtpClient(SmtpServerAddress, SmtpServerPort)
            {
                Credentials = Credentials,
                EnableSsl = true
            })
            {
                client.Send(mailMessage);
                Console.WriteLine($"An email was sent to {email} with subject: \"{subject}\".");
            }
        }
    }
}