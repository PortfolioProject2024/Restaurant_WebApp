using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Restaurant_WebApp.Data;

namespace Restaurant_WebApp.Models.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;


        public EmailSender(IConfiguration configuration, ApplicationDbContext db,
             UserManager<User> userManager)
        {
            _configuration = configuration;
            _db = db;
            _userManager = userManager;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string smtpHost = _configuration["SmtpSettings:Host"];
            int smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");
            string smtpUsername = _configuration["SmtpSettings:Username"];
            string smtpPassword = _configuration["SmtpSettings:Password"];
            bool enableSsl = _configuration.GetValue<bool>("SmtpSettings:EnableSsl");
            string noReplyAddress = "theflavorloft@gmail.com";

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.Body = htmlMessage;
                mailMessage.IsBodyHtml = true;

                // Set the "Reply-To" header to the no-reply address
                mailMessage.ReplyToList.Add(new MailAddress(noReplyAddress));

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email. Error: " + ex.Message);
                }
            }

            return Task.CompletedTask;
        }
    }
}
