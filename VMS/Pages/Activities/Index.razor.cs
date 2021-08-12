using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Activities
{
    public partial class Index
    {
        private List<ActivityViewModel> activities;
        private bool isLoggedIn;

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            activities = await ActivityService.GetAllActivitiesAsync();
            isLoggedIn = IdentityService.IsLoggedIn();
        }
    }
}
