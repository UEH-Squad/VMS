using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class Rightrail : ComponentBase
    {
        private ViewActivityViewModel activity;
        private List<string> Skills { get; set; } = new List<string>();

        [CascadingParameter] public IModalService Modal { get; set; }

        [Parameter]
        public string ActivityId { get; set; }

        [Inject]
        public IActivityService ActivityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(int.Parse(ActivityId));
        }

        private void ShowReportPopUp()
        {
            ModalParameters parameters = new();
            parameters.Add("ActivityId", ActivityId);

            ModalOptions options = new()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            Modal.Show<PopUpReport>("", parameters, options);
        }

        private void ShowSignUpPopUp()
        {
            ModalOptions options = new()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            Modal.Show<ActivitySearchPage.Signup>("", options);
        }
    }
}