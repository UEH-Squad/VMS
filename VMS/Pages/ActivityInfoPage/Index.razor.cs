using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class Index : ComponentBase
    {
        ViewActivityViewModel activity;
        bool isLoading;

        [Parameter]
        public int ActivityId { get; set; }

        [Inject]
        IActivityService ActivityService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public string UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            
            activity = await ActivityService.GetViewActivityViewModelAsync(ActivityId);

            if (activity is null || !activity.IsApproved && !activity.OrgId.Equals(UserId))
            {
                NavigationManager.NavigateTo("404");
                return;
            }

            isLoading = false;
        }
    }
}