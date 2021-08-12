using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Activities
{
    public partial class View
    {
        private ViewActivityViewModel activity;
        private string userId;

        [Parameter]
        public string ActivityId { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetViewActivityViewModelAsync(int.Parse(ActivityId));
            userId = IdentityService.GetCurrentUserId();
        }
    }
}
