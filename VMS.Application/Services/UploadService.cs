using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const long MaxFileSize = 1024 * 1024 * 15;
        private const int MaxWidthFile = 2048;
        private const int MaxHeightFile = 2048;
        private const string FormatFile = "image/jpeg";

        public UploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> GetDataUriAsync(IBrowserFile file)
        {
            file = await file.RequestImageFileAsync(FormatFile, MaxWidthFile, MaxHeightFile);

            using Stream stream = file.OpenReadStream(MaxFileSize);
            using MemoryStream memoryStream = new();
            await stream.CopyToAsync(memoryStream);

            return $"data:image/jpeg;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
        }

        public async Task<string> SaveImageAsync(IBrowserFile file, string userId)
        {
            file = await file.RequestImageFileAsync(FormatFile, MaxWidthFile, MaxHeightFile);

            string fileName = $"{DateTime.Now.ToFileTime()}_{userId}.jpg";
            string pathFile = @$"{_webHostEnvironment.WebRootPath}\img";

            using FileStream fileStream = File.Create(@$"{pathFile}\{fileName}");
            using Stream stream = file.OpenReadStream(MaxFileSize);
            await stream.CopyToAsync(fileStream);

            return fileName;
        }

        public void RemoveImage(string fileName)
        {
            string path = @$"{_webHostEnvironment.WebRootPath}\img\{fileName}";
            File.Delete(path);
        }
    }
}
