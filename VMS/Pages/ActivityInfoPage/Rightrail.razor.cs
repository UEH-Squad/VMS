﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        string activityAddress;

        [CascadingParameter] public IModalService Modal { get; set; }
        
        [CascadingParameter] public string CurrentUserId { get; set; }

        [Parameter] public ViewActivityViewModel Activity { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            currentUser = IdentityService.GetUserWithFavoritesAndRecruitmentsById(CurrentUserId);
            if (currentUser is not null)
            {
                isAlreadySignedUp = currentUser.Recruitments.Any(x => x.ActivityId == Activity.Id);
            }
        }

        protected override void OnParametersSet()
        {
            string fullAddress = Activity.AddressPaths.Reverse().Aggregate("", (acc, next) => acc + ", " + next.Name);
            activityAddress = !string.IsNullOrEmpty(Activity.Address)
                ? $"{Activity.Address}{fullAddress}"
                : fullAddress.Trim(',', ' ');
            if (!string.IsNullOrEmpty(Activity.Targets))
            {
                targets.Clear();
                targets.AddRange(Activity.Targets?.Split('|'));
            }
        }

        private bool HasValidUser()
        {
            // TODO: Show edit profile pop-up and redirect user to edit org profile page

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

        private async Task ShowSignUpPopUpAsync()
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

            await Modal.Show<ActivitySearchPage.Signup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            OnInitialized();
        }

        private void ShowRequireSignup()
        {
            Modal.Show<Shared.Components.RequireSignup>("", BlazoredModalOptions.GetModalOptions());
        }

        private bool IsSignupTimeExpired()
        {
            return isAlreadySignedUp || !(Activity.OpenDate <= DateTime.Now.Date && DateTime.Now.Date <= Activity.CloseDate) || Activity.IsClosed;
        }
    }
}