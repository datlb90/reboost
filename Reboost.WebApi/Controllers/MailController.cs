
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.Shared;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
//using MimeKit;
//using MimeKit.Cryptography;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using System.Net.Mail;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Org.BouncyCastle.Crypto;
using System.IO;
using System.Net.Mail;
using System.Text;
using Limilabs.Mail;
using System.Security.Cryptography;
using Limilabs.Cryptography;
using Limilabs.Client.SMTP;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mime;
using DKIM;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController: ControllerBase
    {
        private IConfiguration _configuration;
        public MailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("maildll")]
        public async Task<IActionResult> SendMaildllEmail()
        {
            try
            {
                string toAddress = HttpContext.Request.Form["toAddress"];
                RSACryptoServiceProvider rsa = new Limilabs.Cryptography.PemReader().ReadPrivateKeyFromFile("/Users/badat.le/Documents/Projects/reboost/Reboost.WebApi/wwwroot/dkimKey.txt");
                IMail email = Limilabs.Mail.Fluent.Mail.Text("Test Dkim")
                       .From("dat@edvision.ai")
                       .To(toAddress)
                       .Subject("Test Dkim Email")
                       .DKIMSign(rsa, "rb", "edvision.ai")
                       .Create();

                using (Smtp smtp = new Smtp())
                {
                    var emailCredentials = _configuration.GetSection("EmailNetWork");
                    await smtp.ConnectAsync(emailCredentials["Host"], 25);  // or ConnectSSL for SSL
                    await smtp.UseBestLoginAsync(emailCredentials["Username"], emailCredentials["Password"]);
                    await smtp.SendMessageAsync(email);
                    smtp.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(ex.Message);
            }
        }

        [HttpPost("custom")]
        public async Task<IActionResult> SendCustomSignedEmail()
        {
            try
            {
                string toAddress = HttpContext.Request.Form["toAddress"];
                var emailCredentials = _configuration.GetSection("EmailNetWork");
                string smtp = emailCredentials["Host"];
                string from = "dat@edvision.ai";
                string subject = "dkim test email";
                string to = toAddress;
                string body = "This is the body of the message." + Environment.NewLine + "This is the second line";
                string base64privatekey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAjDgfxPRQ+K44LyVRdKsTXbjcn4rkgTbPH/QLlBhDXywc/+Vd
1Y/nEafTZaTm2KtbdgedpflqNVSl0FoEc1zqwQqwo6zN/51Zc23vpAivVNUVo1Np
FKpjOc1c4N0jdHvZBUGZXDbD2DOGvOEILDugO9WO4Bw3ICCP9vUku+K8SDPzxM5a
sOXTKM9a6p+PgI/1uk+IarhVokUkTjynLrw2U7vsR6V8fOMSUjlkUZQ5DLH4rrhU
V6/bTN4p/YYmxaYeFT52mQc7Oi1VkWGU7xSHrnVZqQNCMRideAq0bUS7xuTWXu2M
rfrw1+0ebkbJtyYLXnMNYQoYGctVei7ks3TOwQIDAQABAoIBAFd3dxHT2WKWwsNe
AwbE5Iq1EyKOcGXN+7wR7tsNm5EVom1Z9YOPhXM0/iBMkhecJKxmqcdr4v2rdNeY
H2ibEHEkqvUeiU2nKLJhZHgtVrmTRjEgic7zTnwzB3nZWBGEY+bsea7VFquZ0LWn
pgJ5m0VXvQ09bIGk7kj9uFFfaVWb6ycy++sxgfmedO9KT7owUXPtNeSrMYoYo5o1
uCL2IU40X58u/KrvxPWsp2Uql8Xk/VEJWWnfroITl7UTf9e5F70Ev6lYTaYaorkF
3gvndcJNYZoFp4/wJD4/CW8G2GZp5XCsqaKU0EqFyYkH7EwhnQUzpUJ5Kp2/eT+C
D2oy+wECgYEA3z+yQelfuCw5ordQKgasOYF5biHT5thZRh0WwRTdP7uTXsxCcYeI
LYUAG3IeFzfeLVx9O33+t2Fg8fvfLeNIrb59/gkerU4+5lL5gct6MHEluwcnQ+qv
r4em98srGeWNktSse9EtJctH5EvkA4Y3TW7rk/10oAh8CQ6k8mSZAjECgYEAoMot
+wVYv4WfFyQxI07RmKOs/kZ0sOIXhDnE1zlvuNeIW3aZTfz5J+jZJH3JH7SZD5V+
cWHZA8wD6twXS6x8iigfEWGc882VZShfc5H7OtFCC0h117QEUl/CRwZURibV5xoZ
hnGrYKktlviDxFsjuXQDBacMf362xerB3grWYZECgYB3xVyFMAdCb7ecLGy9n0bW
szfKdiuRNZDvpLuIawEoKCIwQbWD4wqUIT6letvZ6QcxnuUw/a+iAoRYTqC1pbJg
REqj95CVudzVBL8He3raclmjtXrDXS1UJprVZhKBmj4SADdpVhe6pwyy1mRF38Pb
rx6EDv56vRKOaWTlBkNwgQKBgBNa4xgRd7JuUk1F1QhsB3z5Tuy47HkSbkRDc/d4
eYNlpotkBmZF6nQsal8jKR/A7J/cngDmB2qWl24hGkjItaEn3T2JY4xRlgc8Seku
jHBzGiEjktPpXo/P6SIFmAVtzVfpY2M0sa2MD+nZdnsfgXhkh6yZhD6gsT232ahx
44aRAoGBALE4o6yl/EQgw/p55eNnuoBi862cuFPvweXTNZxLmWjVwDhGNAR/dI9H
skL4EXXBKsP1FdyoH590mw5kYR7f1vbyWl5aqZPzVFRFaQhlE7nO9brZH3TFKiY2
fY3FHBh0j68SlfHLU8swnRlkPjwq5N377BQ7XYib0ZxsC0JfgIg5
-----END RSA PRIVATE KEY-----";

                HashAlgorithm hash = new SHA256Managed();
                // HACK!! simulate the quoted-printable encoding SmtpClient will use
                string hashBody = body.Replace(Environment.NewLine, "=0D=0A") + Environment.NewLine;
                byte[] bodyBytes = Encoding.ASCII.GetBytes(hashBody);
                string hashout = Convert.ToBase64String(hash.ComputeHash(bodyBytes));
                // timestamp  - seconds since 00:00:00 on January 1, 1970 UTC
                TimeSpan t = DateTime.Now.ToUniversalTime() - DateTime.SpecifyKind(DateTime.Parse("00:00:00 January 1, 1970"), DateTimeKind.Utc);

                string signatureHeader = "v=1; " +
                 "a=rsa-sha256; " +
                 "c=relaxed/relaxed; " +
                 "d=edvision.ai; " +
                 "s=rb; " +
                 "t=" + Convert.ToInt64(t.TotalSeconds) + "; " +
                 "bh=" + hashout + "; " +
                 "h=From:To:Subject:Content-Type:Content-Transfer-Encoding; " +
                 "b=";

                string canonicalizedHeaders =
                "from:" + from + Environment.NewLine +
                "to:" + to + Environment.NewLine +
                "subject:" + subject + Environment.NewLine +
                @"content-type:text/plain; charset=us-ascii
content-transfer-encoding:quoted-printable
dkim-signature:" + signatureHeader;


                TextReader reader = new StringReader(base64privatekey);
                Org.BouncyCastle.OpenSsl.PemReader r = new Org.BouncyCastle.OpenSsl.PemReader(reader);
                AsymmetricCipherKeyPair o = r.ReadObject() as AsymmetricCipherKeyPair;
                byte[] plaintext = Encoding.ASCII.GetBytes(canonicalizedHeaders);
                ISigner sig = SignerUtilities.GetSigner("SHA256WithRSAEncryption");
                sig.Init(true, o.Private);

                sig.BlockUpdate(plaintext, 0, plaintext.Length);
                byte[] signature = sig.GenerateSignature();
                signatureHeader += Convert.ToBase64String(signature);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;

                message.Headers.Add("DKIM-Signature", signatureHeader);
                SmtpClient client = new SmtpClient();
                //client.Send(message);


                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(emailCredentials["Username"], emailCredentials["Password"]);
                client.Host = emailCredentials["Host"];
                client.Port = Int32.Parse(emailCredentials["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                await client.SendMailAsync(message);


                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(ex.Message);
            }
           
        }


        [HttpPost("dkimnet")]
        public async Task<IActionResult> SendDkimNetEmail()
        {
            try
            {
                string toAddress = HttpContext.Request.Form["toAddress"];
                var emailCredentials = _configuration.GetSection("EmailNetWork");
                SmtpClient client = new SmtpClient();
                MailMessage email = new MailMessage();
                email.To.Clear();
                email.To.Add(new MailAddress(toAddress));
                email.From = new MailAddress("dat@edvision.ai");


                email.Subject = "Test";
                email.IsBodyHtml = true;
                email.Body = "Test";

                // Configure our email server
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(emailCredentials["Username"], emailCredentials["Password"]);
                client.Host = emailCredentials["Host"];
                client.Port = Int32.Parse(emailCredentials["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                // DKIM settings
                string domain = "edvision.ai";
                // NCSU DKIM selector
                string selector = "rb";
                // NCSU DKIM private key
                var privateKey = PrivateKeySigner.Create(@"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAjDgfxPRQ+K44LyVRdKsTXbjcn4rkgTbPH/QLlBhDXywc/+Vd
1Y/nEafTZaTm2KtbdgedpflqNVSl0FoEc1zqwQqwo6zN/51Zc23vpAivVNUVo1Np
FKpjOc1c4N0jdHvZBUGZXDbD2DOGvOEILDugO9WO4Bw3ICCP9vUku+K8SDPzxM5a
sOXTKM9a6p+PgI/1uk+IarhVokUkTjynLrw2U7vsR6V8fOMSUjlkUZQ5DLH4rrhU
V6/bTN4p/YYmxaYeFT52mQc7Oi1VkWGU7xSHrnVZqQNCMRideAq0bUS7xuTWXu2M
rfrw1+0ebkbJtyYLXnMNYQoYGctVei7ks3TOwQIDAQABAoIBAFd3dxHT2WKWwsNe
AwbE5Iq1EyKOcGXN+7wR7tsNm5EVom1Z9YOPhXM0/iBMkhecJKxmqcdr4v2rdNeY
H2ibEHEkqvUeiU2nKLJhZHgtVrmTRjEgic7zTnwzB3nZWBGEY+bsea7VFquZ0LWn
pgJ5m0VXvQ09bIGk7kj9uFFfaVWb6ycy++sxgfmedO9KT7owUXPtNeSrMYoYo5o1
uCL2IU40X58u/KrvxPWsp2Uql8Xk/VEJWWnfroITl7UTf9e5F70Ev6lYTaYaorkF
3gvndcJNYZoFp4/wJD4/CW8G2GZp5XCsqaKU0EqFyYkH7EwhnQUzpUJ5Kp2/eT+C
D2oy+wECgYEA3z+yQelfuCw5ordQKgasOYF5biHT5thZRh0WwRTdP7uTXsxCcYeI
LYUAG3IeFzfeLVx9O33+t2Fg8fvfLeNIrb59/gkerU4+5lL5gct6MHEluwcnQ+qv
r4em98srGeWNktSse9EtJctH5EvkA4Y3TW7rk/10oAh8CQ6k8mSZAjECgYEAoMot
+wVYv4WfFyQxI07RmKOs/kZ0sOIXhDnE1zlvuNeIW3aZTfz5J+jZJH3JH7SZD5V+
cWHZA8wD6twXS6x8iigfEWGc882VZShfc5H7OtFCC0h117QEUl/CRwZURibV5xoZ
hnGrYKktlviDxFsjuXQDBacMf362xerB3grWYZECgYB3xVyFMAdCb7ecLGy9n0bW
szfKdiuRNZDvpLuIawEoKCIwQbWD4wqUIT6letvZ6QcxnuUw/a+iAoRYTqC1pbJg
REqj95CVudzVBL8He3raclmjtXrDXS1UJprVZhKBmj4SADdpVhe6pwyy1mRF38Pb
rx6EDv56vRKOaWTlBkNwgQKBgBNa4xgRd7JuUk1F1QhsB3z5Tuy47HkSbkRDc/d4
eYNlpotkBmZF6nQsal8jKR/A7J/cngDmB2qWl24hGkjItaEn3T2JY4xRlgc8Seku
jHBzGiEjktPpXo/P6SIFmAVtzVfpY2M0sa2MD+nZdnsfgXhkh6yZhD6gsT232ahx
44aRAoGBALE4o6yl/EQgw/p55eNnuoBi862cuFPvweXTNZxLmWjVwDhGNAR/dI9H
skL4EXXBKsP1FdyoH590mw5kYR7f1vbyWl5aqZPzVFRFaQhlE7nO9brZH3TFKiY2
fY3FHBh0j68SlfHLU8swnRlkPjwq5N377BQ7XYib0ZxsC0JfgIg5
-----END RSA PRIVATE KEY-----");

                // Setup DKIM signer
                var domainKeySigner = new DomainKeySigner(privateKey, domain, selector, new string[] { "From", "To", "Subject" });
                email.DomainKeySign(domainKeySigner);

                var dkimSigner = new DKIM.DkimSigner(privateKey, domain, selector, new string[] { "From", "To", "Subject" });
                email.DkimSign(dkimSigner);

                // Send DKIM email
                //new SmtpClient().Send(email);
                await client.SendMailAsync(email);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(ex.Message);
            }

        }


        //[HttpPost("mime")]
        //public async Task<IActionResult> SendMimeEmail()
        //{
        //    try
        //    {
        //        string toAddress = HttpContext.Request.Form["toAddress"];
        //        var message = new MimeMessage();
        //        message.To.Add(new MailboxAddress("Dat", toAddress));
        //        message.From.Add(new MailboxAddress("Dat", "dat@edvision.ai"));
        //        message.Subject = "Test";


        //        DkimSigner Signer = new DkimSigner(
        //            "/Users/badat.le/Documents/Projects/reboost/Reboost.WebApi/wwwroot/dkimKey.txt", // path to your privatekey
        //            "edvision.ai", // your domain name
        //            "rb") // The selector given on https://dkimcore.org/
        //        {
        //            HeaderCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
        //            BodyCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
        //            AgentOrUserIdentifier = "@reboost.vn", // your domain name
        //            QueryMethod = "dns/txt",
        //        };

        //        var headers = new HeaderId[] { HeaderId.From, HeaderId.Subject, HeaderId.To };


        //        var builder = new BodyBuilder();
        //        builder.TextBody = "Test";
        //        builder.HtmlBody = "<p>Test</p>";

        //        message.Body = builder.ToMessageBody();
        //        message.Prepare(EncodingConstraint.SevenBit);

        //        Signer.Sign(message, headers);

        //        using (var client = new SmtpClient())
        //        {
        //            var emailCredentials = _configuration.GetSection("EmailNetWork");
        //            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
        //            client.ServerCertificateValidationCallback = (s, c, h, e) =>
        //            {
        //                Console.WriteLine(e);
        //                return true;
        //            };
        //            //Int32.Parse(emailCredentials["Port"])
        //            await client.ConnectAsync(emailCredentials["Host"], 25);
        //            await client.AuthenticateAsync(emailCredentials["Username"], emailCredentials["Password"]);
        //            string rs = await client.SendAsync(message);
        //            client.Disconnect(true);
        //        }


        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return Ok(ex.Message);
        //    }

        //}
    }
}

