using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;

namespace VMS.Pages.Admin.DetailReportPage
{
    public partial class DetailReport : ComponentBase
    {
        private string reasons;
        private ReportViewModel report = new();

        [Parameter] public int ReportId { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        [CascadingParameter] public string CurrentUserId { get; set; }

        [Inject] private IReportService ReportService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetDataAsync();
        }

        private async Task SetDataAsync()
        {
            report = await ReportService.GetReportByIdAsync(ReportId);
            reasons = ResolveReportPage.ListReport.ConvertListReasonsToString(report.Reasons);
        }

        private async Task ChangeReportStateAsync(ReportState state)
        {
            if (state.Equals(ReportState.Closed) || state.Equals(ReportState.Deleted))
            {
                var parameters = new ModalParameters();
                parameters.Add("ActionDelete", state.Equals(ReportState.Deleted));
                parameters.Add("IsClosed", report.IsClosed);
                var result = await Modal.Show<ConfirmAction>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

                if ((bool)result.Data)
                {
                    await ReportService.UpdateReportStateAsync(ReportId, state, CurrentUserId);
                }
            }
            else
            {
                var parameters = new ModalParameters();
                parameters.Add("State", state);
                await Modal.Show<Notification>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

                await ReportService.UpdateReportStateAsync(ReportId, state, CurrentUserId);
            }

            await SetDataAsync();
        }

        private void ShowImage(string imageUrl)
        {
            var parameters = new ModalParameters();
            parameters.Add("Image", imageUrl);
            Modal.Show<WatchImageReport>("", parameters, BlazoredModalOptions.GetModalOptions());
        }
    }
}
