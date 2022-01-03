using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.Admin.WatchRating
{
    public partial class Index : ComponentBase
    {
        [Parameter]
        public int ActId { get; set; }
        private string actName;
        private ViewActivityViewModel actViewModel;
        private int page = 1;
        private PaginatedList<RecruitmentViewModel> pagedResult = new(new(), 0, 1, 1);
        private FilterRecruitmentViewModel filter = new();

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.actViewModel = await ActivityService.GetViewActivityViewModelAsync(ActId);
            actName = actViewModel.Name;
            pagedResult = await RecruitmentService.GetAllRatingAsync(ActId, filter, page);
        }

        private void FilterChanged(FilterRecruitmentViewModel filter)
        {
            this.filter = filter;
            this.filter.IsSearch = false;
        }
        private async Task HandlePageChangedAsync()
        {
            pagedResult = await RecruitmentService.GetAllRatingAsync(ActId, filter, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }
    }
}
