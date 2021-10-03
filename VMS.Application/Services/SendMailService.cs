using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

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

        public async Task SendConfirmEmail(string userEmail, string callbackUrl)
        {
            MailMessage mailMessage = new(systemEmail, userEmail)
            {
                Sender = new MailAddress(systemEmail, systemName),
                Subject = "GoVirlunteer - Xác nhận tài khoản",
                Body = "Vui lòng xác nhận tài khoản GoVirlunteer của bạn bằng cách click vào <a href=" + callbackUrl + ">đây</a>.",
                IsBodyHtml = true
            };

            await SendEmail(mailMessage);
        }

        private async Task SendEmail(MailMessage message)
        {
            using SmtpClient smtpClient = new(systemHost, systemPort)
            {
                Credentials = new NetworkCredential()
                {
                    UserName = systemEmail,
                    Password = systemEmailPassword
                },
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
            message.Dispose();
        }
    }
}