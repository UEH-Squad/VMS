using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> GetDataUriAsync(IBrowserFile file);
        Task<string> SaveImageAsync(IBrowserFile file, string userId);
        void RemoveImage(string fileName);
    }
}
