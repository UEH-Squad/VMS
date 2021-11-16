using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;
using VMS.Common.Enums;

namespace VMS.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> GetDataUriAsync(IBrowserFile file);
        Task<string> SaveImageAsync(IBrowserFile file, string userId, ImgFolder imgFolder);
        void RemoveImage(string fileName);
    }
}
