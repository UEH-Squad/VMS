using Microsoft.AspNetCore.Components;
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

        [Inject] 
        private IReportService ReportService { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        public PopUpReport()
        {
            report = new ReportViewModel();
        }
        private async Task AddReport()
        {
            report.UserId = IdentityService.GetCurrentUserId();

            await ReportService.AddReport(report);
        }
    }
}
