using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Activities
{
    public partial class Delete
    {
        private CreateActivityViewModel activity;
        private string userId;

        [Parameter]
        public string ActivityId { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IUploadService UploadService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetCreateActivityViewModelAsync(int.Parse(ActivityId));
            userId = IdentityService.GetCurrentUserId();
        }
        private async Task DeleteActivityAsync()
        {
            if (!string.IsNullOrEmpty(activity.Banner))
            {
                UploadService.RemoveImage(activity.Banner);
            }

            await ActivityService.DeleteActivityAsync(int.Parse(ActivityId));
            NavigationManager.NavigateTo(Routes.Activities);
        }
    }
}
