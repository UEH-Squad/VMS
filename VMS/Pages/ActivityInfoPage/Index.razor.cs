using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

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

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            activity = await ActivityService.GetViewActivityViewModelAsync(ActivityId);
            isLoading = false;
        }
    }
}