using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class ActList : ComponentBase
    {
        private bool isLoading;

        private int page;
        private int menuId;
        private PaginatedList<ActivityViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter]
        public FilterActivityViewModel Filter { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        private async Task InitDataAsync()
        {
            isLoading = true;
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page);
            isLoading = false;
        }

        protected override async Task OnParametersSetAsync()
        {
            page = 1;
            await InitDataAsync();
        }

        private async Task HandlePageChangedAsync()
        {
            await InitDataAsync();
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private async Task ShowDeleteModalAsync(ActivityViewModel activity)
        {
            var result = await Modal.Show(typeof(Organization.Profile.DeleteConfirm), "", BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data)
            {
                await ActivityService.CloseOrDeleteActivity(activity.Id, true, activity.IsClosed);
                await InitDataAsync();
            }
        }

        private void ShowMenu(int id)
        {
            menuId = menuId == id ? 0 : id;
        }

        private void ShowEditModal(int id)
        {
            ModalParameters parameters = new();
            parameters.Add("ActId", id);
            Modal.Show<EditRequirement>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task ShowApproveModalAsync(int activityId)
        {
            var result = await Modal.Show<ApprovalActivity>("", BlazoredModalOptions.GetModalOptions()).Result;

            var approveResult = (ApprovalActivity.ApproveResult)result.Data;

            if (approveResult.IsApprove)
            {
                await ActivityService.ApproveActAsync(activityId, approveResult.IsPoint, approveResult.IsDay, approveResult.NumberOfDays);
                await InitDataAsync();
            }
        }

        [JSInvokable]
        public Task HideMenuInterop()
        {
            menuId = 0;
            return InvokeAsync(StateHasChanged);
        }

        private async Task ShowActPrivorModalAsync(ActivityViewModel activity)
        {
            List<ActivityViewModel> pinnedActivities = await ActivityService.GetFeaturedActivitiesAsync();

            if (activity.IsPin)
            {
                pinnedActivities.ForEach(x => x.IsPin = activity.Id != x.Id);
            }
            else
            {
                if (pinnedActivities.Count >= 3)
                {
                    var parameters = new ModalParameters();
                    parameters.Add("ListPinned", pinnedActivities);
                    await Modal.Show<PriorActivity>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
                }

                activity.IsPin = pinnedActivities.FindAll(x => x.IsPin).Count < 3;
                pinnedActivities.Add(activity);
            }

            await ActivityService.ChangePinStateListActivityAsync(pinnedActivities);

            await InitDataAsync();
        }

        private async Task ShowDenyModalAsync(int id)
        {
            var result = await Modal.Show<PopupDenyAct>("", BlazoredModalOptions.GetModalOptions()).Result;
            if ((bool)result.Data == true)
            {
                await ActivityService.CloseOrDeleteActivity(id, true, true);
                await InitDataAsync();
            }
        }
    }
}
