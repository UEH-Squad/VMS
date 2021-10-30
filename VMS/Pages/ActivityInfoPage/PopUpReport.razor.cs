using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class PopUpReport : ComponentBase
    {
        private bool showListReason = false;
        private bool isShowReport = false;
        private readonly string space = " ";
        private readonly ReportViewModel report;
        private IReadOnlyList<IBrowserFile> selectedImages;

        [CascadingParameter]
        private BlazoredModalInstance ReportModal { get; set; }

        [Parameter]
        public int ActivityId { get; set; }

        [Inject]
        private IReportService ReportService { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private IUploadService UploadService { get; set; }

        public PopUpReport()
        {
            report = new ReportViewModel();
        }

        private List<string> Reason { get; set; } = new List<string>();
        private List<string> Image { get; set; } = new List<string>();

        private async Task AddReport()
        {
            report.UserId = IdentityService.GetCurrentUserId();
            report.ActivityId = ActivityId;
            report.Reasons = Reason;
            report.Images = Image;

            await ReportService.AddReportAsync(report);
        }

        private static List<string> Reasons => new()
        {
            "Hoạt động không có thật",
            "Hoạt động không không tương thích với nội dung đã đề cập",
            "Hình ảnh, video không phù hợp/không liên quan tới nội dung hoạt động",
            "Nội dung hoạt động không phù hợp",
            "Lĩnh vực và kỹ năng không phù hợp với nội dung hoạt động",
            "Khác"
        };

        private void CheckboxClicked(string reason, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!Reason.Contains(reason))
                {
                    Reason.Add(reason);
                }
            }
            else
            {
                if (Reason.Contains(reason))
                {
                    Reason.Remove(reason);
                }
            }
        }

        private bool isChangeFile = false;

        private async Task OnInputFile(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            selectedImages = imageFiles;
            Image.Clear();
            isChangeFile = true;
            foreach (var file in imageFiles)
            {
                if (file.ContentType != "image/jpeg")
                {
                    this.StateHasChanged();
                }
                else
                {
                    string x = await UploadService.SaveImageAsync(file, IdentityService.GetCurrentUserId());
                    Image.Add(x);
                }
            }
        }

        private void CloseModal()
        {
            ReportModal.CloseAsync();
        }

        private void ShowModalReportSucess()
        {
            isShowReport = !isShowReport;
            if (isShowReport == false)
            {
                ReportModal.CloseAsync();
            }
        }

        private void ListReason()
        {
            showListReason = !showListReason;
        }

        private void TextArea()
        {
            showListReason = false;
        }
    }
}