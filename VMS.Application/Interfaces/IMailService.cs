using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface IMailService
    {
        Task SendConfirmEmail(string userEmail, string callbackUrl);
        Task SendForgotPasswordEmail(string userEmail, string callbackUrl, string fullName, string role);
    }
}