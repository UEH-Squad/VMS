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
        public string ActId { get; set; }
        private int actId;
        private string actName;
        private ViewActivityViewModel actViewModel;
        [Inject] 
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            this.actId = int.Parse(ActId);
            this.actViewModel = await ActivityService.GetViewActivityViewModelAsync(actId);
            string orgId = IdentityService.GetCurrentUserId();
            if(orgId == actViewModel.OrgId)
            {
                actName = actViewModel.Name;
            }
            else
            {
                NavigationManager.NavigateTo(Routes.HomePage, true);
            }
        }
    }
}
