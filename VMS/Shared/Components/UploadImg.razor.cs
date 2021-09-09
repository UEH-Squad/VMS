using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace VMS.Shared.Components
{
    /// <summary>
    /// UploadImg component
    /// </summary>
    public partial class UploadImg : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; }

        #region Parameters

        /// <summary>
        /// Initial prompt text when the whole component shown up
        /// </summary>
        [Parameter]
        public string DefaultPrompt { get; set; } = "Chọn hoặc kéo và thả hình ảnh liên quan tại đây!";

        /// <summary>
        /// Toggle the preview img panel
        /// </summary>
        [Parameter]
        public bool ShowPreview { get; set; }

        /// <summary>
        /// The src attribute value of the &lt;img&gt; used in preview
        /// </summary>
        [Parameter]
        public string PreviewImgSrc { get; set; }

        /// <summary>
        /// file filter when choosing file via open file dialog of operating system, default is "image/*"
        /// </summary>
        [Parameter]
        public string AcceptPattern { get; set; } = "image/*";

        /// <summary>
        /// Upload File selection change event
        /// </summary>
        [Parameter]
        public EventCallback<InputFileChangeEventArgs> InputFileChanged { get; set; }

        [Parameter]
        public Func<Task> ImageDiscarded { get; set; }

        #endregion Parameters

        private string prompt;
        private string fileName;
        private string previewImgAltText;

        private readonly string fileUploadId = Guid.NewGuid().ToString();
        private readonly string imgContainerId = Guid.NewGuid().ToString();
        private ElementReference previewImg;
        private ElementReference discardBtn;

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(prompt) && !string.IsNullOrEmpty(DefaultPrompt))
            {
                prompt = DefaultPrompt;
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShowPreview)
            {
                await JS.InvokeVoidAsync("vms.HookFileUploadEvent", previewImg, fileUploadId, discardBtn, imgContainerId, PreviewImgSrc);
            }
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            previewImgAltText = e.File.Name;
            if (ShowPreview)
            {
                fileName = previewImgAltText;
            }
            else
            {
                prompt = previewImgAltText;
            }

            await InputFileChanged.InvokeAsync(e);
        }

        private void OnDiscardImage()
        {
            previewImgAltText = fileName = null;
            ImageDiscarded?.Invoke();
        }
    }
}