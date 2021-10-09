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

        private void ResetState()
        {
            rateId = 0;
            dropdownId = 0;
            confirmCloseId = 0;
            confirmDeleteId = 0;
            popupClose = 0;
            popupDelete = 0;
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

        private async Task ChangeDeleteStateAsync(ActivityViewModel activity)
        {
            if (confirmDeleteId == activity.Id)
            {
                await ActivityService.UpdateStatusActAsync(activity.Id, activity.IsClosed, !activity.IsDeleted);
                ResetState();
                popupDelete = activity.Id;
                await HandlePageChangedAsync();
            }
            else
            {
                confirmDeleteId = activity.Id;
            }
        }

        private async Task ChangeCloseStateAsync(ActivityViewModel activity)
        {
            if (confirmCloseId == activity.Id)
            {
                await ActivityService.CloseOrDeleteActivity(activity.Id, activity.IsDeleted, !activity.IsClosed);
                ResetState();
                popupClose = activity.Id;
                await HandlePageChangedAsync();
            }
            else
            {
                confirmCloseId = activity.Id;
            }
        }

        private bool CheckForZIndex(int id)
        {
            return id == dropdownId || id == rateId || id == confirmCloseId || id == confirmDeleteId || id == popupClose || id == popupDelete;
        }

        private async Task ShowDeleteModal(int id)
        {
            var parameters = new ModalParameters();
            parameters.Add("IsOrgProfile", IsOrgProfile);
            parameters.Add("ActId", id);
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };
            var result = await Modal.Show(typeof(Pages.Organization.Profile.DeleteConfirm), "", parameters, options).Result;
        }
        private async Task ShowCloseModal(int id)
        {
            var act = data.Items.Find(a => a.Id == id);

            var modalParams = new ModalParameters();
            modalParams.Add("IsClosed", act.IsClosed);

            var parameters = new ModalParameters();
            parameters.Add("ActId", id);
            parameters.Add("IsClosed", act.IsClosed);
            parameters.Add("IsOrgProfile", IsOrgProfile);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };
            var result = await Modal.Show(typeof(Pages.Organization.Profile.CloseConfirm), "", parameters, options).Result;

            if ((bool)result.Data)
            {
                act.IsClosed = !act.IsClosed;
                Modal.Show(typeof(Pages.Organization.Profile.CloseSuccess), "", modalParams, options);
            }
        }
    }
}
