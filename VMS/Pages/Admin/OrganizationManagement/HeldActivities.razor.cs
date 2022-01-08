using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Pages.Admin.OrganizationManagement
{
    public partial class HeldActivities : ComponentBase
    {
        private int page;
        private PaginatedList<ActivityViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter] public string UserId { get; set; }

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] private IActivityService ActivityService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            page = 1;
            pagedResult = await ActivityService.GetAllOrganizationActivityViewModelAsync(new() { OrgId = UserId }, 1);
        }

        private async Task HandlePageChangedAsync()
        {
            pagedResult = await ActivityService.GetAllOrganizationActivityViewModelAsync(new() { OrgId = UserId }, page);
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
