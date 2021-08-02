using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> ImageProcessing(IBrowserFile browserFile, string activityBanner);
    }
}
