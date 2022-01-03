using System;
using VMS.Common;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components;

namespace VMS.Pages.Organization.RatingPage
{
    public partial class Index : ComponentBase
    {
        private bool? isRated;
        private string searchValue;
        private int starRating;
        private UserViewModel currentUser;
        private ViewActivityViewModel activity;

        [Parameter] public int ActivityId { get; set; }

        [CascadingParameter] public string UserId { get; set; }

        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            currentUser = OrganizationService.GetOrgFull(UserId);

            activity = await ActivityService.GetViewActivityViewModelAsync(ActivityId);

            if (activity is null)
            {
                NavigationManager.NavigateTo("404");
                return;
            }

            if (activity.OrgId != currentUser.Id)
            {
                NavigationManager.NavigateTo(Routes.HomePage, true);
            }

            if (activity.EndDate >= DateTime.Now.Date)
            {
                NavigationManager.NavigateTo(Routes.ActivityManagement, true);
            }
        }

        private void IsRatedChanged(bool? value)
        {
            isRated = value;
            searchValue = string.Empty;
        }

        private void SearchValueChanged(string value)
        {
            searchValue = value;
            isRated = null;
        }
    }
}
