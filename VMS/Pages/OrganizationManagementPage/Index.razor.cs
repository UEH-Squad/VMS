using Microsoft.AspNetCore.Components;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.OrganizationManagementPage
{
    public partial class Index : ComponentBase
    {
        [Parameter]
        public FilterActivityViewModel filter { get; set; } = new();

        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override void OnInitialized()
        {
            filter.OrgId = IdentityService.GetCurrentUserId();
        }
    }
}
