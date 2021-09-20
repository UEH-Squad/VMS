using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class Rightrail : ComponentBase
    {
        readonly List<string> targets = new();
        User currentUser;

        [CascadingParameter] public IModalService Modal { get; set; }

        [Parameter]
        public ViewActivityViewModel Activity { get; set; }

        protected override void OnInitialized()
        {
            currentUser = IdentityService.GetCurrentUser();
        }

        protected override void OnParametersSet()
        {
            string fullAddress = Activity.AddressPaths.Reverse().Aggregate("", (acc, next) => acc + ", " + next.Name);
            Activity.Address = !string.IsNullOrEmpty(Activity.Address)
                ? $"{Activity.Address}{fullAddress}"
                : fullAddress.Trim(',', ' ');
            if (!string.IsNullOrEmpty(Activity.Targets))
            {
                targets.AddRange(Activity.Targets?.Split('|'));
            }
        }

        private void ShowReportPopUp()
        {
            if (currentUser is null)
            {
                // TODO: Show login pop-up or edit info pop-up

                return;
            }

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
            if (currentUser is null)
            {
                // TODO: Show login pop-up or edit info pop-up

                return;
            }

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