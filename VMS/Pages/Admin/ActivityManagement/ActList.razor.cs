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

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            page = 1;
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page);
            isLoading = false;
        }

        private async Task HandlePageChangedAsync()
        {
            isLoading = true;
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page);
            isLoading = false;
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private async Task ShowDeleteModalAsync(ActivityViewModel activity)
        {
            var result = await Modal.Show(typeof(Organization.Profile.DeleteConfirm), "", BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data)
            {
                await ActivityService.CloseOrDeleteActivity(activity.Id, true, activity.IsClosed);
                await OnParametersSetAsync();
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
                await OnParametersSetAsync();
            }
        }

        [JSInvokable]
        public Task HideMenuInterop()
        {
            menuId = 0;
            return InvokeAsync(StateHasChanged);
        }

        private async Task ShowActPrivorModalAsync(int id)
        {
            List<int> pin = new();
            pin.Add(id);
            List<ActivityViewModel> list = await ActivityService.GetActivityIsPin();
            bool isPin = pagedResult.Items.Find(x => x.Id == id).IsPin;
            if (isPin == false && list.Count >= 3)
            {
                ModalParameters parameters = new();
                parameters.Add("ListPinned", list);
                var result = await Modal.Show<PriorActivity>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
                List<int> listUnpin = (List<int>)result.Data;
                if (listUnpin != null)
                {
                    await ActivityService.PinActivityAsync(listUnpin, false);
                    await ActivityService.PinActivityAsync(pin, true);
                    pagedResult.Items.Find(x => x.Id == id).IsPin = true;
                }
            }
            else
            {
                ModalParameters parameters = new();
                parameters.Add("IsPin", !isPin);
                var result = await Modal.Show<PriorActSuccess>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
                if ((bool)result.Data == true)
                {
                    await ActivityService.PinActivityAsync(pin, !isPin);
                    pagedResult.Items.Find(x => x.Id == id).IsPin = !isPin;
                }
            }

        }

        private async Task ShowDenyModalAsync(int id)
        {
            var result = await Modal.Show<PopupDenyAct>("", BlazoredModalOptions.GetModalOptions()).Result;
            if ((bool)result.Data == true)
            {
                await ActivityService.CloseOrDeleteActivity(id, true, true);
            }
        }
    }
}
