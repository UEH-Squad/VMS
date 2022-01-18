using System;
using Microsoft.AspNetCore.Components;
using VMS.Common;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using VMS.Common.Enums;

namespace VMS.Pages.UserProflie
{
    public partial class Index : ComponentBase
    {
        private bool isLoading;
        private bool isUser = false;
        private UserViewModel user;
        private List<ActivityViewModel> currentActivities, favoriteActivities, endedActivities = new();

        [Parameter] public string UserId { get; set; }
        [Parameter] public bool IsUsedForAdmin { get; set; }
        [CascadingParameter] public string CurrentUserId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IActivityService ActivityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ValidateUserProfile();
            await GetAllActivities();
        }

        private async Task GetAllActivities()
        {
            isLoading = true;

            currentActivities = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Current, DateTime.Now);

            favoriteActivities = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Favor, DateTime.Now);

            endedActivities = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Ended, DateTime.Now);

            isLoading = false;
        }

        private void ValidateUserProfile()
        {
            UserId = string.IsNullOrEmpty(UserId) ? CurrentUserId : UserId;

            user = UserService.GetUserViewModel(UserId);

            if (string.IsNullOrEmpty(UserId) || user is null)
            {
                NavigationManager.NavigateTo("404", true);
                return;
            }

            isUser = string.Equals(UserId, CurrentUserId, StringComparison.Ordinal);

            if (isUser && !IsValidProfile(user))
            {
                NavigationManager.NavigateTo(Routes.EditUserProfile, true);
                return;
            }
        }

        private static bool IsValidProfile(UserViewModel user)
        {
            return !string.IsNullOrEmpty(user.Class)
                && !string.IsNullOrEmpty(user.Email)
                && user.Skills.Count != 0
                && user.Areas.Count != 0;
        }
    }
}
