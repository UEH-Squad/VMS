using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class Rightrail : ComponentBase
    {
        readonly List<string> targets = new();
        User currentUser;
        bool isAlreadySignedUp;

        [CascadingParameter] public IModalService Modal { get; set; }

        [Parameter]
        public ViewActivityViewModel Activity { get; set; }

        protected override void OnInitialized()
        {
            currentUser = IdentityService.GetCurrentUserWithFavoritesAndRecruitments();
            if (currentUser is not null)
            {
                isAlreadySignedUp = currentUser.Recruitments.Any(x => x.UserId == currentUser.Id);
            }
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

        private bool HasValidUser()
        {
            // TODO: Check user profile

            return true;
        }

        private void ShowReportPopUp()
        {
            if (currentUser is null)
            {
                ShowRequireSignup();

                return;
            }

            if (!HasValidUser())
            {
                // TODO: Show edit profile pop-up and redirect user to edit org profile page
            }

            ModalParameters parameters = new();
            parameters.Add("ActivityId", Activity.Id);

            Modal.Show<PopUpReport>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private void ShowSignUpPopUp()
        {
            if (currentUser is null)
            {
                ShowRequireSignup();
                return;
            }

            if (!HasValidUser())
            {
                // TODO: Show edit profile pop-up and redirect user to edit org profile page
            }

            ModalParameters parameters = new();
            parameters.Add("ActivityId", Activity.Id);

            Modal.Show<ActivitySearchPage.Signup>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private void ShowRequireSignup()
        {
            Modal.Show<Shared.Components.RequireSignup>("", BlazoredModalOptions.GetModalOptions());
        }
    }
}