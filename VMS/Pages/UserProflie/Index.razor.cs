using Microsoft.AspNetCore.Components;
using VMS.Common;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using System.Threading.Tasks;
using VMS.Application.Services;
using System.Collections.Generic;

namespace VMS.Pages.UserProflie
{
    public partial class Index : ComponentBase
    {
        private bool isUser, isLoggedIn = false;
        private UserViewModel user;
        private List<ActivityViewModel> currentActivities, favoriteActivities, endedActivities = new();

        [Parameter] public string UserId { get; set; }
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
            currentActivities = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Current);

            favoriteActivities = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Favor);
            favoriteActivities.ForEach(a => a.IsFav = true);

            endedActivities = await ActivityService.GetAllUserActivityViewModelsAsync(UserId, StatusAct.Ended);
        }

        private void ValidateUserProfile()
        {
            if (string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(CurrentUserId))
            {
                NavigationManager.NavigateTo(Routes.LogIn, true);
            }

            UserId = string.IsNullOrEmpty(UserId) ? CurrentUserId : UserId;

            user = UserService.GetUserViewModel(UserId);

            if (user == null)
            {
                NavigationManager.NavigateTo(Routes.HomePage, true);
            }

            isUser = string.Equals(UserId, CurrentUserId, System.StringComparison.Ordinal);

            if (isUser && !IsValidProfile(user))
            {
                NavigationManager.NavigateTo(Routes.EditUserProfile, true);
            }

            isLoggedIn = !string.IsNullOrEmpty(CurrentUserId);
        }

        private static bool IsValidProfile(UserViewModel user)
        {
            return !string.IsNullOrEmpty(user.Class)
                && !string.IsNullOrEmpty(user.Email)
                && !string.IsNullOrEmpty(user.PhoneNumber)
                && user.Skills.Count != 0
                && user.Areas.Count != 0;
        }
    }
}
