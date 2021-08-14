using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
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
            activities = await ActivityService.GetAllActivitiesAsync(new FilterActivityViewModel());
            isLoggedIn = IdentityService.IsLoggedIn();
        }
        private async void GetFilter(FilterActivityViewModel filter)
        {
            activities = await ActivityService.GetAllActivitiesAsync(filter);
            StateHasChanged();
        }

        private void OrderActivities(ChangeEventArgs e)
        {
            switch ((string)e.Value)
            {
                case "Nearest":
                    break;
                case "Hottest":
                    activities = activities.OrderByDescending(a => a.MemberQuantity).ToList();
                    break;
                default:
                    activities = activities.OrderByDescending(a => a.PostDate).ToList();
                    break;
            }
        }
    }
}
