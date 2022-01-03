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
        private int page = 1;
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
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page);
        }

        private async Task HandlePageChangedAsync()
        {
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }
        private async Task ShowDeleteModalAsync(int id)
        {
           var result = await Modal.Show(typeof(Pages.Organization.Profile.DeleteConfirm), "", BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data)
            {
                var act = pagedResult.Items.Find(x => x.Id == id);
                await ActivityService.CloseOrDeleteActivity(id, true, act.IsClosed);
                //pagedResult.Items.Remove(act);
                pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page);
                StateHasChanged();
            }
        }
        void ShowMenu(int id)
        {
            pagedResult.Items.ForEach(a => a.IsMenu = a.Id == id && !a.IsMenu);
        }

        private async Task ShowEditModalAsync(int id)
        {
            ModalParameters parameters = new();
            parameters.Add("ActId", id);
            Modal.Show<EditRequirement>("",parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task ShowApproveModalAsync(int id)
        {
            var result = await Modal.Show<ApprovalActivity>("", BlazoredModalOptions.GetModalOptions()).Result;
            List<object> list = (List<object>)result.Data;
                await ActivityService.ApproveActAsync(id,(bool)list[0], (bool)list[1],(int)list[2]);
                pagedResult.Items.Find(x => x.Id == id).IsApproved = true;
                pagedResult.Items.Find(x => x.Id == id).IsDay = (bool)list[1];
                pagedResult.Items.Find(x => x.Id == id).IsPoint = (bool)list[0];

        }
        [JSInvokable]
        public Task HideMenuInterop()
        {
            pagedResult.Items.ForEach(a => a.IsMenu = false);
            return InvokeAsync(StateHasChanged);
        }

        private async Task ShowActPrivorModalAsync(int id)
        {
            List<int> pin = new();
            pin.Add(id);
            List<ActivityViewModel> list = await ActivityService.GetActivityIsPin();
            bool isPin = pagedResult.Items.Find(x => x.Id == id).IsPin;
            if (isPin ==false && list.Count >=3)
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
                var result = await Modal.Show<PriorActSuccess>("",parameters, BlazoredModalOptions.GetModalOptions()).Result;
                if ((bool)result.Data == true)
                {
                    await ActivityService.PinActivityAsync(pin, !isPin);
                    pagedResult.Items.Find(x => x.Id == id).IsPin = !isPin;
                }
            }
           
        }
    }
}
