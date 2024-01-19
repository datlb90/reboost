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
        Task SendSupportEmail(string fromEmail, string fromName, string subject, string content, List<IFormFile> attachments);
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
            //var apiKey = _configuration["SendGridAPIKey"];
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("reboost.ad@gmail.com", "Reboost Support");
            //var to = new EmailAddress(toEmail);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            //var response = await client.SendEmailAsync(msg);
            //Console.Write(response);

            SmtpClient emailClient = new SmtpClient();
            MailMessage email = new MailMessage();
            email.To.Clear();
            email.Attachments.Clear();
            email.To.Add(new MailAddress(toEmail));
            email.From = new MailAddress("Reboost Support", "support@reboost.vn");
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

        public async Task SendSupportEmail(string fromEmail, string fromName, string subject, string content, List<IFormFile> attachments)
        {
            SmtpClient emailClient = new SmtpClient();
            MailMessage email = new MailMessage();
            email.From = new MailAddress("support@reboost.vn", "Reboost Support");
            email.To.Clear();
            email.To.Add(new MailAddress(fromEmail, fromName));
            email.To.Add(new MailAddress("support@reboost.vn", "Reboost Support"));
            
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = content;

            email.Attachments.Clear();
            foreach (var item in attachments)
            {
                var attachment = new Attachment(item.OpenReadStream(), item.FileName);
                email.Attachments.Add(attachment);
            }
            var emailCredentials = _configuration.GetSection("EmailNetWork");
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = new System.Net.NetworkCredential(emailCredentials["Username"], emailCredentials["Password"]);
            emailClient.Port = Int32.Parse(emailCredentials["Port"]);
            emailClient.Host = emailCredentials["Host"];
            emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            emailClient.EnableSsl = true;
            await emailClient.SendMailAsync(email);
        }

        //public async Task SendContactEmail(string fromEmail, string fromName, string subject, string content, List<String> listPaths)
        //{
        //    var apiKey = _configuration["SendGridAPIKey"];
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("reboost.ad@gmail.com", "Reboost Contact Email");
        //    var to = new EmailAddress("emailforevaluate@gmail.com");
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

        //    if (listPaths.Count > 0)
        //    {
        //        foreach (var p in listPaths)
        //        {
        //            if (File.Exists(p))
        //            {
        //                System.Net.Mail.Attachment attachment;
        //                attachment = new System.Net.Mail.Attachment(p);
        //                msg.AddAttachment(GetSendGridAttachment(attachment));
        //            }
        //        }
        //    }

        //    var response = await client.SendEmailAsync(msg);
        //    Console.Write(response);
        //}

        // Convert from System.Net.Mail.Attachment to SendGrid.Helpers.Mail.Attachment
        //public static Attachment GetSendGridAttachment(System.Net.Mail.Attachment attachment)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        try
        //        {
        //            attachment.ContentStream.CopyTo(stream);
        //            return new Attachment()
        //            {
        //                Disposition = "attachment",
        //                Type = attachment.ContentType.MediaType,
        //                Filename = attachment.Name,
        //                ContentId = attachment.ContentId,
        //                Content = Convert.ToBase64String(stream.ToArray())
        //            };
        //        }
        //        finally
        //        {
        //            stream.Close();
        //        }
        //    }
        //}
    }
}
