using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.SearchOrganizer
{
    public partial class SearchResult : ComponentBase
    {
        [Parameter]
        public EventCallback<int> OnPageChanged { get; set; }
        [Parameter]
        public PaginatedList<UserViewModel> PagedResult { get; set; } = new(new(), 0, 1, 1);

        [CascadingParameter] public IModalService Modal { get; set; }

        private async Task HandlePageChangedAsync(int page)
        {
            await OnPageChanged.InvokeAsync(page);
            await Interop.ScrollToTop(JsRuntime);
        }

        private void ShowHeldActivitiesAsync(string userId)
        {
            if (IsUsedForAdmin)
            {
                var parameters = new ModalParameters();
                parameters.Add("UserId", userId);

                Modal.Show<Admin.OrganizationManagement.HeldActivities>("", parameters, BlazoredModalOptions.GetModalOptions());
            }
        }
    }
}
