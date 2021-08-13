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
        private FilterActivityViewModel filter;

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            filter = new FilterActivityViewModel();
            activities = await ActivityService.GetAllActivitiesAsync();
            isLoggedIn = IdentityService.IsLoggedIn();
        }
        private void GetFilter(FilterActivityViewModel filter)
        {
            this.filter = filter;
        }
    }
}
