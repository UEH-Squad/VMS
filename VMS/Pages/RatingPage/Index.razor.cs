using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.RatingPage
{
    public partial class Index : ComponentBase
    {
        private bool? isRated;
        private string searchValue;
        private int starRating;
        private string currentUserId;
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
            currentUserId = IdentityService.GetCurrentUserId();
            
            activity = await ActivityService.GetViewActivityViewModelAsync(ActivityId);

            if (activity.OrgId != currentUserId)
            {
                NavigationManager.NavigateTo(Routes.Organizations + "/" + currentUserId);
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
