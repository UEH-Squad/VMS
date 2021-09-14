using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Linq;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class Rightrail : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        [Parameter]
        public ViewActivityViewModel Activity { get; set; }

        protected override void OnParametersSet()
        {
            string fullAddress = Activity.AddressPaths.Reverse().Aggregate("", (acc, next) => acc + ", " + next.Name);
            Activity.Address = !string.IsNullOrEmpty(Activity.Address)
                ? $"{Activity.Address}{fullAddress}"
                : fullAddress.Trim(',', ' ');
        }

        private void ShowReportPopUp()
        {
            ModalParameters parameters = new();
            parameters.Add("ActivityId", Activity.Id);

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