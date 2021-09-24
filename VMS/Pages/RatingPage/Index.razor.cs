using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.RatingPage
{
    public partial class Index : ComponentBase
    {
        private bool? isRated;
        private string searchValue;
        private int starRating;
        private User currentUser;
        private ViewActivityViewModel activity;

        [Parameter] public int ActivityId { get; set; }

        [Inject]
        private IIdentityService IdentityService {  get; set; }
        [Inject]
        private IActivityService ActivityService {  get; set; }
        [Inject]
        private NavigationManager NavigationManager {  get; set; }

        protected override async Task OnInitializedAsync()
        {
            currentUser = IdentityService.GetCurrentUser();
            
            activity = await ActivityService.GetViewActivityViewModelAsync(ActivityId);

            if (activity.OrgId != currentUser.Id)
            {
                NavigationManager.NavigateTo(Routes.Organizations + "/" + currentUser.Id);
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
