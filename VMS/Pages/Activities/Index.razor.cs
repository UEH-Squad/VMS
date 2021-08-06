using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Activities
{
    public partial class Index
    {
        private List<ActivityViewModel> activities;
        private IdentityUser user;

        [Inject]
        protected IActivityService ActivityService { get; set; }
        [Inject]
        protected IIdentityService IdentityService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            activities = await ActivityService.GetAllActivitiesAsync();
            user = IdentityService.GetCurrentUser();
        }
    }
}
