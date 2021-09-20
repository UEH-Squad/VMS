using Microsoft.AspNetCore.Components;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.OrganizationManagementPage
{
    public partial class Index : ComponentBase
    {
        private FilterActivityViewModel filter = new();
        private string searchValue = string.Empty;
        private bool isSearch = false;

        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override void OnInitialized()
        {
            filter.OrgId = IdentityService.GetCurrentUserId();
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
            isSearch = !string.IsNullOrEmpty(searchValue);
        }

        private void FilterChanged(FilterActivityViewModel filter)
        {
            this.filter = filter;
            isSearch = false;
        }
    }
}
