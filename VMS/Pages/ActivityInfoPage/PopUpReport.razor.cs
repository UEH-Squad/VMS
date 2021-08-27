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
