using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;
using Blazored.Modal.Services;
using Blazored.Modal;

namespace VMS.Pages.Organization.ActivityManagementPage
{
    public partial class ActivitiesCard : ComponentBase
    {
        private bool isLoading;
        private int dropdownId;
        private int page = 1;
        private PaginatedList<ActivityViewModel> data = new(new(), 0, 1, 1);

        [Parameter]
        public FilterOrgActivityViewModel Filter { get; set; }
        [Parameter]
        public bool IsOrgProfile { get; set; } = false;

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JS.InvokeVoidAsync("vms.AddOutsideClickMenuHandler", DotNetObjectReference.Create(this), nameof(HideMenuInterop));
        }

        [JSInvokable]
        public Task HideMenuInterop()
        {
            dropdownId = 0;
            return InvokeAsync(StateHasChanged);
        }

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            data = await ActivityService.GetAllOrganizationActivityViewModelAsync(Filter, 1);
            isLoading = false;
        }

        private async Task HandlePageChangedAsync(bool isPaging = false)
        {
            isLoading = true;
            data = await ActivityService.GetAllOrganizationActivityViewModelAsync(Filter, page);
            isLoading = false;
            StateHasChanged();
            if (isPaging)
            {
                await Interop.ScrollToTop(JsRuntime);
            }
        }

        private void ChangeDropdownState(int id)
        {
            dropdownId = (dropdownId == id ? 0 : id);
        }

        private async Task ShowDeleteModalAsync(ActivityViewModel activity)
        {
            var result = await Modal.Show(typeof(Profile.DeleteConfirm), "", BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data)
            {
                await ActivityService.CloseOrDeleteActivity(activity.Id, !activity.IsDeleted, activity.IsClosed);
                await HandlePageChangedAsync();
            }
        }

        private async Task ShowCloseModalAsync(ActivityViewModel activity)
        {
            var parameters = new ModalParameters();
            parameters.Add("IsClosed", activity.IsClosed);

            var result = await Modal.Show(typeof(Profile.CloseConfirm), "", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data)
            {
                await ActivityService.CloseOrDeleteActivity(activity.Id, activity.IsDeleted, !activity.IsClosed);
                activity.IsClosed = !activity.IsClosed;
                Modal.Show(typeof(Profile.CloseSuccess), "", parameters, BlazoredModalOptions.GetModalOptions());
            }
        }
    }
}
