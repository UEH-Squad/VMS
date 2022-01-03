using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Common.Enums;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class UploadMultipleImg : ComponentBase
    {
        [Parameter]
        public List<string> Image { get; set; }
        [Parameter]
        public EventCallback<List<string>> ImageChange { get; set; }
        [Inject]
        private IUploadService UploadService { get; set; }
        private async Task OnInputFileAsync(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            foreach (var file in imageFiles)
            {
                if (file.ContentType != "image/jpeg")
                {
                    StateHasChanged();
                }
                else
                {
                    string x = await UploadService.SaveImageAsync(file, "editrequirement", ImgFolder.Activities);
                    Image.Add(x);
                    await ImageChange.InvokeAsync(Image);
                }
            }
        }
        public string DefaultPrompt { get; set; } = "Chọn hoặc kéo và thả hình ảnh liên quan tại đây!";
        private string prompt;
        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(prompt) && !string.IsNullOrEmpty(DefaultPrompt))
            {
                prompt = DefaultPrompt;
            }
        }
        private async Task OnDiscardImageAsync(string img)
        {
            Image.Remove(img);
            await ImageChange.InvokeAsync(Image);
        }

       
    }
}
