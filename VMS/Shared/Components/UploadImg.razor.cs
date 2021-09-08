using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
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

        #endregion Parameters

        private string _prompt;
        private string _previewImgAltText;

        private readonly string _fileUploadId = Guid.NewGuid().ToString();
        private ElementReference previewImg;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (string.IsNullOrEmpty(_prompt) && !string.IsNullOrEmpty(DefaultPrompt))
            {
                _prompt = DefaultPrompt;
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (ShowPreview)
                {
                    await JS.InvokeVoidAsync("vms.HookFileUploadEvent", previewImg, _fileUploadId);
                }
                else
                {
                    await JS.InvokeVoidAsync("vms.HookFileUploadEvent", null, _fileUploadId);
                }
            }
        }

        private Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            _previewImgAltText = _prompt = e.File.Name;

            return InputFileChanged.InvokeAsync(e);
        }
    }
}