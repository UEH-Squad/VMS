using Microsoft.AspNetCore.Components;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.ActivityManagementPage
{
    public partial class Index : ComponentBase
    {
        private string orgId;
        private FilterOrgActivityViewModel filter = new();
        private string searchValue = string.Empty;
        private bool isSearch = false;

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
            this.searchValue = searchValue;
            isSearch = !string.IsNullOrEmpty(searchValue);
        }

        private void FilterChanged(FilterOrgActivityViewModel filter)
        {
            this.filter = filter;
            isSearch = false;
        }
    }
}
