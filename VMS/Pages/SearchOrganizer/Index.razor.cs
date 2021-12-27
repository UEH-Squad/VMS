using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Pages.SearchOrganizer
{
    public partial class Index : ComponentBase
    {
        private FilterOrgViewModel filter = new();
        private PaginatedList<UserViewModel> pagedResult = new(new(), 0, 1, 1);

        [Inject] private IOrganizationService OrganizationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await OnPageChangedAsync(1);
        }

        private async Task OnFilterChangedAsync(FilterOrgViewModel filter)
        {
            this.filter = filter;
            await OnPageChangedAsync(1);
        }

        private async Task OnPageChangedAsync(int page)
        {
            pagedResult = await OrganizationService.GetAllOrganizers(filter, page);
        }
    }
}
