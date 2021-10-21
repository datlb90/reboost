using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
        Task SendContactEmail(string fromEmail, string fromName, string subject, string content, List<String> listPaths);
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
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("adm.reboost@gmail.com", "Reboost Confirmation Email");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
            Console.Write(response);
        }

        public async Task SendContactEmail(string fromEmail, string fromName, string subject, string content, List<String> listPaths)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("adm.reboost@gmail.com", "Reboost Contact Email");
            var to = new EmailAddress("emailforevaluate@gmail.com");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            if (listPaths.Count > 0)
            {
                foreach (var p in listPaths)
                {
                    if (File.Exists(p))
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(p);
                        msg.AddAttachment(GetSendGridAttachment(attachment));
                    }
                }
            }

            var response = await client.SendEmailAsync(msg);
            Console.Write(response);
        }

        // Convert from System.Net.Mail.Attachment to SendGrid.Helpers.Mail.Attachment
        public static Attachment GetSendGridAttachment(System.Net.Mail.Attachment attachment)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    attachment.ContentStream.CopyTo(stream);
                    return new Attachment()
                    {
                        Disposition = "attachment",
                        Type = attachment.ContentType.MediaType,
                        Filename = attachment.Name,
                        ContentId = attachment.ContentId,
                        Content = Convert.ToBase64String(stream.ToArray())
                    };
                }
                finally
                {
                    stream.Close();
                }
            }
        }
    }
}
