using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class Index : ComponentBase
    {
        [Parameter]
        public int ActId { get; set; }
        private string actName;
        private ViewActivityViewModel actViewModel;
        [Inject] 
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [CascadingParameter]
        public string CurrentUserId { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            actViewModel = await ActivityService.GetViewActivityViewModelAsync(ActId);

            if (actViewModel is null)
            {
                NavigationManager.NavigateTo("404");
                return;
            }

            actName = actViewModel.Name;
            bool isUserOrg = string.Equals(actViewModel.OrgId, CurrentUserId, System.StringComparison.Ordinal);
            if (!isUserOrg)
            {
                NavigationManager.NavigateTo("404");
                return;
            }
        }
    }
}
