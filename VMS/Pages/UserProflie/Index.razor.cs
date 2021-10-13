using Microsoft.AspNetCore.Components;
using VMS.Common;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;

namespace VMS.Pages.UserProflie
{
    public partial class Index : ComponentBase
    {
        private bool isUser = false;
        private UserViewModel user;

        [Parameter] public string UserId { get; set; }
        [CascadingParameter] public string CurrentUserId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IUserService UserService { get; set; }

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(CurrentUserId))
            {
                NavigationManager.NavigateTo(Routes.LogIn, true);
            }

            UserId = string.IsNullOrEmpty(UserId) ? CurrentUserId : UserId;

            isUser = string.Equals(UserId, CurrentUserId, System.StringComparison.Ordinal);

            user = UserService.GetUserViewModel(UserId);

            if (user == null)
            {
                NavigationManager.NavigateTo(Routes.HomePage, true);
            }

            if (isUser && !IsValidProfile(user))
            {
                NavigationManager.NavigateTo(Routes.EditUserProfile, true);
            }
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
