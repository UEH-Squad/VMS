using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class PopUpReport : ComponentBase
    {
        private ReportViewModel report;
        private string message;
        private string image;
        private IBrowserFile file;

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

        private string[] reason = new string[]
        {
            "Hoạt động không có thật",
            "Hoạt động không không tương thích với nội dung đã đề cập",
            "Hình ảnh, video không phù hợp/không liên quan tới nội dung hoạt động",
            "Nội dung hoạt động không phù hợp",
            "Lĩnh vực và kỹ năng không phù hợp với nội dung hoạt động",
            "Khác"
        };

        private async Task AddReport()
        {
            report.UserId = IdentityService.GetCurrentUserId();
            report.ActivityId = int.Parse(ActivityId);

            if (file is not null)
            {
                report.ImageReport = await UploadService.SaveImageAsync(file, report.UserId);
            }

            await ReportService.AddReport(report);
        }

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            if (e.File.ContentType != "image/jpeg")
            {
                message = $"File không đúng định dạng..";
                this.StateHasChanged();
            }
            else
            {
                message = "";
                file = e.File;
                image = await UploadService.GetDataUriAsync(file);
            }
        }
    }
}
