using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Pages.Admin.ActivityManagement;

namespace VMS.Pages.Admin.ActivityInfo
{
    public partial class ActivityInfo : ComponentBase
    {
        private ViewActivityViewModel activity = new();

        [Parameter] public int ActId { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject] private IActivityService ActivityService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(ActId);

            if (activity is null)
            {
                NavigationManager.NavigateTo("404");
                return;
            }
        }

        private void ShowEditModal()
        {
            ModalParameters parameters = new();
            parameters.Add("ActId", ActId);
            Modal.Show<EditRequirement>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task ShowApproveModalAsync()
        {
            await ActivityService.ApproveActAsync(ActId, activity.IsPoint, activity.IsDay, activity.IsDay ? activity.NumberOfDays : 0);
            await OnParametersSetAsync();
        }

        private async Task ShowDenyModalAsync()
        {
            var result = await Modal.Show<PopupDenyAct>("", BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data == true)
            {
                await ActivityService.CloseOrDeleteActivity(ActId, true, true);
                NavigationManager.NavigateTo(Routes.AdminActivityManagement);
            }
        }

        private void ShowPopupConvert()
        {
            Modal.Show<PopupConvert>("", BlazoredModalOptions.GetModalOptions());
        }
    }
}
