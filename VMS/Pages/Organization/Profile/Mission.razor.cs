using Microsoft.AspNetCore.Components;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.Profile
{
    public partial class Mission : ComponentBase
    {
        private UserViewModel org;
        [Parameter]
        public string UserId { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        protected override void OnInitialized()
        {
            org = OrganizationService.GetOrgFull(UserId);
        }
    }
}