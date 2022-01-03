using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Admin.ActivityInfo
{
    public partial class ActivityInfo : ComponentBase
    {
        [Parameter]
        public int ActId { get; set; }

        ViewActivityViewModel activity =new();
        [CascadingParameter] public IModalService Modal { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(ActId);
        }
        private async Task ShowEditModal()
        {
            ModalParameters parameters = new();
            parameters.Add("ActId", ActId);
            Modal.Show<VMS.Pages.Admin.ActivityManagement.EditRequirement>("",parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task ShowApproveModal()
        {
            var result = await Modal.Show<VMS.Pages.Admin.ActivityManagement.ApprovalActivity>("", BlazoredModalOptions.GetModalOptions()).Result;
            List<object> list = (List<object>)result.Data;
            await ActivityService.ApproveAct(ActId, (bool)list[0], (bool)list[1], (int)list[2]);
            activity.IsDay = (bool)list[1];
            activity.IsPoint = (bool)list[0];
            activity.NumberOfDay = (int)list[2];
        }

        private async Task ShowDenyModal()
        {
            var result = await Modal.Show<VMS.Pages.Admin.ActivityManagement.PopupDenyAct>("", BlazoredModalOptions.GetModalOptions()).Result;
            if((bool)result.Data == true)
            {
                activity.IsDay = false;
                activity.IsPoint = false;
                activity.NumberOfDay = 0;
                await ActivityService.DenyAct(ActId);
            }
        }

        private async Task ShowPopupConvert()
        {
            Modal.Show<PopupConvert>("", BlazoredModalOptions.GetModalOptions());
        }
    }
}
