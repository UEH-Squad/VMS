using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class PopUpReport : ComponentBase
    {
        private ReportViewModel report;
        private IReadOnlyList<IBrowserFile> selectedImages;

        [Inject]
        private IReportService ReportService { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private IUploadService UploadService { get; set; }

        [Parameter]
        public string ActivityId { get; set; }

        public PopUpReport()
        {
            report = new ReportViewModel();
        }

        private List<string> Reason { get; set; } = new List<string>();
        private List<string> Image { get; set; } = new List<string>();

        private async Task AddReport()
        {
            report.UserId = IdentityService.GetCurrentUserId();
            report.ActivityId = int.Parse(ActivityId);
            report.Reasons = Reason;
            report.Images = Image;

            await ReportService.AddReport(report);
        }

        private List<string> Reasons()
        {
            List<string> c = new List<string>();
            c.Add("Hoạt động không có thật");
            c.Add("Hoạt động không không tương thích với nội dung đã đề cập");
            c.Add("Hình ảnh, video không phù hợp/không liên quan tới nội dung hoạt động");
            c.Add("Nội dung hoạt động không phù hợp");
            c.Add("Lĩnh vực và kỹ năng không phù hợp với nội dung hoạt động");
            c.Add("Khác");
            return c;
        }

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

    }
}
