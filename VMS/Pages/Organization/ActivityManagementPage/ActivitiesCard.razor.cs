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
        private int rateId;
        private int dropdownId;
        private int confirmDeleteId;
        private int confirmCloseId;
        private int popupDelete;
        private int popupClose;
        private int page = 1;
        private PaginatedList<ActivityViewModel> data = new(new(), 0, 1, 1);

        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
        [Parameter]
        public bool IsSearch { get; set; } = false;
        [Parameter]
        public string SearchValue { get; set; } = "";
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
            data.Items.ForEach(a => a.IsMenu = false);
            return InvokeAsync(StateHasChanged);
        }

        void ShowMenu(int id)
        {
            data.Items.ForEach(a => a.IsMenu = a.Id == id && !a.IsMenu);
        }

        protected override async Task OnParametersSetAsync()
        {
            data = await ActivityService.GetAllActivitiesAsync(IsSearch, SearchValue, Filter, page);
        }

        private async Task HandlePageChangedAsync(bool isPaging = false)
        {
            data = await ActivityService.GetAllActivitiesAsync(IsSearch, SearchValue, Filter, page);
            StateHasChanged();
            if (isPaging)
            {
                await Interop.ScrollToTop(JsRuntime);
            }
        }

        private void ChangeRateState(int id)
        {
            rateId = (rateId == id ? 0 : id);
            dropdownId = 0;
        }

        private void ChangeDropdownState(int id)
        {
            rateId = 0;
            dropdownId = (dropdownId == id ? 0 : id);
        }

        private bool CheckForZIndex(int id)
        {
            return id == dropdownId || id == rateId;
        }

        private async Task ShowDeleteModalAsync(ActivityViewModel activity)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };

            var result = await Modal.Show(typeof(Pages.Organization.Profile.DeleteConfirm), "", options).Result;

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

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };

            var result = await Modal.Show(typeof(Pages.Organization.Profile.CloseConfirm), "", parameters, options).Result;

            if ((bool)result.Data)
            {
                await ActivityService.CloseOrDeleteActivity(activity.Id, activity.IsDeleted, !activity.IsClosed);
                activity.IsClosed = !activity.IsClosed;
                Modal.Show(typeof(Pages.Organization.Profile.CloseSuccess), "", parameters, options);
            }
        }
    }
}
