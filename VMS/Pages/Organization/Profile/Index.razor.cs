using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;

namespace VMS.Pages.Organization.Profile
{
    public partial class Index : ComponentBase
    {
        private UserViewModel org = new();
        private List<ActivityViewModel> actCurrent = new();
        private List<ActivityViewModel> actFavorite = new();
        private List<ActivityViewModel> actEnded = new();
        public bool owner;
        public bool haveControl;
        public bool haveFav;
        public bool haveLogin;
        
        [Parameter]
        public string UserId { get; set; }

        [CascadingParameter]
        public string CurrentUserId { get; set; }

        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        
        [Inject]
        private IActivityService ActivityService { get; set; }
        
        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(UserId))
            {
                NavigationManager.NavigateTo(Routes.LogIn, true);
            }

            UserId = string.IsNullOrEmpty(UserId) ? CurrentUserId : UserId;

            org = OrganizationService.GetOrgFull(UserId);

            if (org == null)
            {
                NavigationManager.NavigateTo(Routes.HomePage, true);
            }

            bool isUserOrg = string.Equals(UserId, CurrentUserId, System.StringComparison.Ordinal);

            if (isUserOrg)
            {
                if (!CheckInforUser(org))
                {
                    NavigationManager.NavigateTo(Routes.EditOrgProfile);
                }

                haveFav = false;
                haveControl = true;
            }

            actCurrent = await ActivityService.GetOrgActsAsync(UserId, StatusAct.Current);
            actFavorite = await ActivityService.GetOrgActsAsync(UserId, StatusAct.Favor);
            actEnded = await ActivityService.GetOrgActsAsync(UserId, StatusAct.Ended);

            if (string.IsNullOrEmpty(CurrentUserId))
            {
                // anonymous user
                haveFav = true;
                haveLogin = false;
                haveControl = false;
                return;
            }

            if (!isUserOrg)
            {
                haveFav = true;
                haveControl = false;
            }

            haveLogin = true;
        }

        private static bool CheckInforUser(UserViewModel org) => !string.IsNullOrEmpty(org.FullName)
                                                                 && !string.IsNullOrEmpty(org.Email)
                                                                 && !string.IsNullOrEmpty(org.PhoneNumber)
                                                                 && !string.IsNullOrEmpty(org.Mission)
                                                                 && !string.IsNullOrEmpty(org.Banner)
                                                                 && org.Areas.Count != 0;
    }
}