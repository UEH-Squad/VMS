using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;
using VMS.GenericRepository;

namespace VMS.Pages.Admin.ResolveReportPage
{
    public partial class ListReport : ComponentBase
    {
        private bool isLoading;
        private PaginatedList<ReportViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter] public FilterReportViewModel Filter { get; set; } = new();

        [CascadingParameter] public string CurrentUserId { get; set; }

        [Inject] private IReportService ReportService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await SetDataAsync();
        }

        private async Task SetDataAsync(int page = 1)
        {
            isLoading = true;
            pagedResult = await ReportService.GetAllReportsAsync(Filter, page);
            isLoading = false;
        }

        private async Task HandlePageChangedAsync(int page)
        {
            await SetDataAsync(page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        public static string ConvertListReasonsToString(List<string> reasons)
        {
            return reasons.Aggregate((a, b) => a + "; " + b);
        }

        private async Task HandlePinAsync(int reportId)
        {
            await ReportService.UpdateReportStateAsync(reportId, ReportState.Pinned, CurrentUserId);
            await SetDataAsync();
        }
    }
}
