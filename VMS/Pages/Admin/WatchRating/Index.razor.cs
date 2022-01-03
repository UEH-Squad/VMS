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
        private int page;
        private string actName;
        private ViewActivityViewModel actViewModel;
        private PaginatedList<RecruitmentViewModel> pagedResult = new(new(), 0, 1, 1);
        private FilterRecruitmentViewModel filter = new();

        [Parameter] public int ActId { get; set; }

        [Inject]
        private IRecruitmentService RecruitmentService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            actViewModel = await ActivityService.GetViewActivityViewModelAsync(ActId);

            if (actViewModel is null)
            {
                NavigationManager.NavigateTo("404");
                return;
            }

            actName = actViewModel.Name;

            page = 1;
            await InitDataAsync();
        }

        private async Task InitDataAsync()
        {
            pagedResult = await RecruitmentService.GetAllRatingAsync(ActId, filter, page);
        }

        private async Task FilterChangedAsync(FilterRecruitmentViewModel filter)
        {
            this.filter = filter;
            this.filter.IsSearch = false;

            page = 1;
            await InitDataAsync();
        }

        private async Task HandlePageChangedAsync()
        {
            await InitDataAsync();
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }
    }
}
