using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface IMailService
    {
        Task SendLoginConfirmEmail(string userEmail, string callbackUrl);
    }
}