using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.Profile
{
    public partial class Index : ComponentBase
    {
        private UserViewModel org;
        private List<ActivityViewModel> actCurrent = new();
        private List<ActivityViewModel> actFavorite = new();
        private List<ActivityViewModel> actEnded;
        public bool owner;
        [Parameter]
        public string UserId { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            org = OrganizationService.GetOrgFull(UserId);
            org = OrganizationService.GetOrgFull(UserId);
            actCurrent = await ActivityService.GetOrgActs(UserId, StatusAct.Current);
            actFavorite = await ActivityService.GetOrgActs(UserId, StatusAct.Favor);
            actEnded = await ActivityService.GetOrgActs(UserId, StatusAct.Ended);
            _ = !string.Equals(UserId, IdentityService.GetCurrentUserId()) ? owner = false : owner = true;
        }
    }
}