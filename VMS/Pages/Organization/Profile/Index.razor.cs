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
        private bool isLoading;

        [Parameter]
        public string UserId { get; set; }
        [Parameter] public bool IsUsedForAdmin { get; set; }


        [CascadingParameter]
        public string CurrentUserId { get; set; }

        [Inject]
        private IOrganizationService OrganizationService { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserId = string.IsNullOrEmpty(UserId) ? CurrentUserId : UserId;

            org = OrganizationService.GetOrgViewModel(UserId);

            if (org is null || string.IsNullOrEmpty(UserId))
            {
                NavigationManager.NavigateTo("404", true);
                return;
            }

            bool isUserOrg = string.Equals(UserId, CurrentUserId, System.StringComparison.Ordinal);

            if (isUserOrg)
            {
                if (!CheckInforUser(org))
                {
                    NavigationManager.NavigateTo(Routes.EditOrgProfile, true);
                }

                haveFav = false;
                haveControl = true;
            }

            isLoading = true;

            actCurrent = await ActivityService.GetOrgActsAsync(UserId, StatusAct.Current);
            actFavorite = await ActivityService.GetOrgActsAsync(UserId, StatusAct.Favor);
            actEnded = await ActivityService.GetOrgActsAsync(UserId, StatusAct.Ended);

            isLoading = false;

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
                                                                 && !string.IsNullOrEmpty(org.Mission)
                                                                 && !string.IsNullOrEmpty(org.Banner)
                                                                 && org.Areas.Count != 0;
    }
}