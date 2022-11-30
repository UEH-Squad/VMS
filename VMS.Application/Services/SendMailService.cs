using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace VMS.Application.Services
{
    public class MailService : IMailService
    {
        private readonly string systemName;
        private readonly string systemEmail;
        private readonly string systemEmailPassword;
        private readonly string systemHost;
        private readonly int systemPort;

        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;

            systemName = _configuration.GetValue<string>("MailSettings:DisplayName");
            systemEmail = _configuration.GetValue<string>("MailSettings:Email");
            systemEmailPassword = _configuration.GetValue<string>("MailSettings:Password");
            systemHost = _configuration.GetValue<string>("MailSettings:Host");
            systemPort = _configuration.GetValue<int>("MailSettings:Port");
        }

        //public async Task SendConfirmEmail(string userEmail, string callbackUrl)
        //{
        //    MailMessage mailMessage = new(systemEmail, userEmail)
        //    {
        //        Sender = new MailAddress(systemEmail, systemName),
        //        Subject = "GoVirlunteer - Xác nhận tài khoản",
        //        Body = "Vui lòng xác nhận tài khoản GoVirlunteer của bạn bằng cách click vào <a href=" + callbackUrl + ">đây</a>.",
        //        IsBodyHtml = true
        //    };

        //    await SendEmail(mailMessage);
        //}

        private async Task SendEmail(MailMessage message)
        {
            using SmtpClient smtpClient = new(systemHost, systemPort);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential()
            {
                UserName = systemEmail,
                Password = systemEmailPassword
            };
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(message);
            message.Dispose();
        }

        // Cong Chien dev
        public async Task SendConfirmEmail(string userEmail, string callbackUrl)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom("govirlunteer@ueh.edu.vn", "Go Virlunteer");
            msg.AddTo(new EmailAddress(userEmail));
            msg.SetTemplateId("d-3bdaece0812841aba4b1bbf2793d0d0d");
            msg.SetTemplateData(new
            {
                Subject = "GoVirlunteer - Xác Nhận Tài Khoản",
                CallbackUrl = callbackUrl
            });
            var response = await client.SendEmailAsync(msg);
            //var temp = response.IsSuccessStatusCode ? "ok" : "db";
        }

        // Cong Chien dev
        public async Task SendForgotPasswordEmail(string userEmail, string callbackUrl, string fullName, string role)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom("govirlunteer@ueh.edu.vn", "Go Virlunteer");
            msg.AddTo(new EmailAddress(userEmail));
            msg.SetTemplateId("d-4a6e30953b6a4e9682467ee2cbb1f6c8");
            msg.SetTemplateData(new
            {
                Subject = "GoVirlunteer - Đặt Lại Mật Khẩu",
                CallbackUrl = callbackUrl,
                Name = fullName,
                Role = role
            });
            var response = await client.SendEmailAsync(msg);
            //var temp = response.IsSuccessStatusCode ? "ok" : "db";
        }
    }
}