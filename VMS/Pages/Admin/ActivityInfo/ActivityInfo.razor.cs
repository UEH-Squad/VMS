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

        protected override async Task OnParametersSetAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(ActId);
        }

        private void ShowEditModal()
        {
            ModalParameters parameters = new();
            parameters.Add("ActId", ActId);
            Modal.Show<EditRequirement>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task ShowApproveModalAsync()
        {
            var approveResult = new ApprovalActivity.ApproveResult
            {
                IsApprove = false,
                IsPoint = activity.IsPoint,
                IsDay = activity.IsDay,
                NumberOfDays = activity.NumberOfDay
            };

            var parameters = new ModalParameters();
            parameters.Add("Result", approveResult);

            await Modal.Show<ApprovalActivity>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            if (approveResult.IsApprove)
            {
                await ActivityService.ApproveActAsync(ActId, approveResult.IsPoint, approveResult.IsDay, approveResult.NumberOfDays);
                await OnParametersSetAsync();
            }
        }

        private async Task ShowDenyModalAsync()
        {
            var result = await Modal.Show<PopupDenyAct>("", BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data == true)
            {
                activity.IsDay = false;
                activity.IsPoint = false;
                activity.NumberOfDay = 0;
                await ActivityService.DenyActAsync(ActId);
            }
        }

        private void ShowPopupConvert()
        {
            Modal.Show<PopupConvert>("", BlazoredModalOptions.GetModalOptions());
        }
    }
}
