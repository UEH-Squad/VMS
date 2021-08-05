using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> GetDataUri(IBrowserFile file);
        Task<string> SaveImage(IBrowserFile file, string userId);
        void RemoveImage(string fileName);
    }
}
