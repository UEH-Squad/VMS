using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.Profile
{
    public partial class Index : ComponentBase
    {
        private List<ActivityViewModel> actCurent = new();
        private List<ActivityViewModel> actFavorite = new();
        public bool Owner;
        [Parameter]
        public string UserId { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            actCurent = await ActivityService.GetOrgActs(UserId, "curent");
            actFavorite = await ActivityService.GetOrgActs(UserId, "favorite");
            if (!string.Equals(UserId, IdentityService.GetCurrentUserId()))
            {
                Owner = false;
            }
            else Owner = true;
        }
    }
}