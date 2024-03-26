using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
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
        private string sendGridKey = "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";

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

            //var client = new SendGridClient(sendGridKey);
            //var from = new EmailAddress("support@reboost.vn", "Reboost");
            //var to = new EmailAddress(toEmail);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            //var response = await client.SendEmailAsync(msg);

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
            if (attachments != null)
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

            //var client = new SendGridClient(sendGridKey);
            //var from = new EmailAddress("support@reboost.vn", "Reboost");
            //var to = new EmailAddress("support@reboost.vn", "Reboost");
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            //msg.AddTo(new EmailAddress(fromEmail, fromName));
            //if (attachments != null)
            //{
            //    foreach (var item in attachments)
            //    {
            //        using (var ms = new MemoryStream())
            //        {
            //            item.CopyTo(ms);
            //            var fileBytes = ms.ToArray();
            //            string fileContent = Convert.ToBase64String(fileBytes);
            //            msg.AddAttachment(item.FileName, fileContent);
            //        }
            //    }
            //}
            //var response = await client.SendEmailAsync(msg);

        }
    }
}
