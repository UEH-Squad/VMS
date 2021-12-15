using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.SearchOrganizer
{
    public partial class SearchResult : ComponentBase
    {
        private int page;
        private PaginatedList<UserViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter] public FilterOrgViewModel Filter { get; set; }

        [Inject] IOrganizationService OrganizationService { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            page = 1;
            pagedResult = await OrganizationService.GetAllOrganizers(Filter, page);
        }

        private async Task HandlePageChangedAsync()
        {
            pagedResult = await OrganizationService.GetAllOrganizers(Filter, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }
    }
}
