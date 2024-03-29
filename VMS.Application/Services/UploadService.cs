﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Common.Enums;

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

        public async Task<string> SaveImageAsync(IBrowserFile file, string userId, ImgFolder imgFolder)
        {
            if (!file.ContentType.Contains("image/"))
            {
                throw new Exception("Invalid file type!");
            }

            file = await file.RequestImageFileAsync(FormatFile, MaxWidthFile, MaxHeightFile);
            string folder = @$"img\{imgFolder}";
            string fileName = @$"{folder}\{DateTime.Now.ToFileTime()}_{userId}.jpg";

            Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, folder));

            using FileStream fileStream = File.Create(Path.Combine(_webHostEnvironment.WebRootPath, fileName));
            using Stream stream = file.OpenReadStream(MaxFileSize);
            await stream.CopyToAsync(fileStream);

            return fileName;
        }

        public void RemoveImage(string fileName)
        {
            try
            {
                File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, fileName));
            }
            catch (Exception ex)
            {
                // ignore if delete exceptions
            }
        }
    }
}