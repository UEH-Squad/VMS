using System.Threading.Tasks;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using System.Collections.Generic;

namespace VMS.Pages.Organization.Profile
{
    public partial class ActTookPlace : ComponentBase
    {
        private List<ActivityViewModel> actEnded;
        [Parameter]
        public string UserId { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            actEnded = await ActivityService.GetOrgActs(UserId, "ended");
        }

        private bool HaftStar(float rate, int star)
        {
            if (rate - star >= 0 && rate - star < 0.5)
            {
                return true;
            }
            return false;
        }
    }

}