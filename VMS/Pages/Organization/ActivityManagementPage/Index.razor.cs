using Microsoft.AspNetCore.Components;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.ActivityManagementPage
{
    public partial class Index : ComponentBase
    {
        private string orgId;
        private FilterOrgActivityViewModel filter = new();

        [CascadingParameter] private string OrgId { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override void OnInitialized()
        {
            orgId = IdentityService.GetCurrentUserId();
            filter.OrgId = orgId;
        }

        private void SearchValueChanged(string searchValue)
        {
            filter.SearchValue = searchValue;
            filter.IsSearch = true;
        }

        private void FilterChanged(FilterOrgActivityViewModel filter)
        {
            this.filter = filter;
            this.filter.IsSearch = false;
        }
    }
}
