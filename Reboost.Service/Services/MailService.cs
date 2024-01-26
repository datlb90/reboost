using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using OpenAI_API.Moderation;
//using SendGrid;
//using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
        Task SendSupportEmail(string fromEmail, string fromName, string subject, string content, List<IFormFile>? attachments);
    }

    public class SendGridMailService : IMailService
    {
        private IConfiguration _configuration;

        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            SmtpClient emailClient = new SmtpClient();
            MailMessage email = new MailMessage();
            email.To.Clear();
            email.Attachments.Clear();
            email.To.Add(new MailAddress(toEmail));
            email.From = new MailAddress("support@reboost.vn", "Reboost");
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = content;

            var emailCredentials = _configuration.GetSection("EmailNetWork");
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = new System.Net.NetworkCredential(emailCredentials["Username"], emailCredentials["Password"]);
            emailClient.Port = Int32.Parse(emailCredentials["Port"]);
            emailClient.Host = emailCredentials["Host"];
            emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            emailClient.EnableSsl = true;
            await emailClient.SendMailAsync(email);
        }

        public async Task SendSupportEmail(string fromEmail, string fromName, string subject, string content, List<IFormFile>? attachments)
        {
            SmtpClient emailClient = new SmtpClient();
            MailMessage email = new MailMessage();
            email.From = new MailAddress("support@reboost.vn", "Reboost");
            email.To.Clear();
            email.To.Add(new MailAddress(fromEmail, fromName));
            email.To.Add(new MailAddress("support@reboost.vn", "Reboost"));
            
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = content;

            email.Attachments.Clear();
            if(attachments != null)
            {
                foreach (var item in attachments)
                {
                    var attachment = new Attachment(item.OpenReadStream(), item.FileName);
                    email.Attachments.Add(attachment);
                }
            }
            var emailCredentials = _configuration.GetSection("EmailNetWork");
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = new System.Net.NetworkCredential(emailCredentials["Username"], emailCredentials["Password"]);
            emailClient.Host = emailCredentials["Host"];
            emailClient.Port = Int32.Parse(emailCredentials["Port"]);
            emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            emailClient.EnableSsl = true;
            await emailClient.SendMailAsync(email);
        }
    }
}
