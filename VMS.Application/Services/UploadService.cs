using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int MaxWidthFile = 820;
        private const int MaxHeightFile = 312;
        private const string FormatFile = "image/jpeg";

        public UploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> ImageProcessing(IBrowserFile browserFile, string activityBanner)
        {
            browserFile = await browserFile.RequestImageFileAsync(FormatFile, MaxWidthFile, MaxHeightFile);
            string fileName = $"{DateTime.Now.ToFileTime()}_{browserFile.Name}";
            string pathFile = $"{_webHostEnvironment.WebRootPath}\\img";

            using (Stream stream = browserFile.OpenReadStream())
            {
                using (FileStream fileStream = File.Create($"{pathFile}\\{fileName}"))
                {
                    await stream.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                stream.Close();
            }

            if (!string.IsNullOrEmpty(activityBanner))
            {
                File.Delete($"{pathFile}\\{activityBanner}");
            }
            return $"{fileName}";
        }
    }
}
