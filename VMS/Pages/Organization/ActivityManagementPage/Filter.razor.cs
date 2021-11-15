using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.ActivityManagementPage
{
    public partial class Filter : ComponentBase
    {
        [Parameter]
        public string OrgId { get; set; }
        [Parameter]
        public FilterOrgActivityViewModel FilterAct { get; set; }
        [Parameter]
        public EventCallback<FilterOrgActivityViewModel> FilterActChanged { get; set; } 

        private async Task OnClickFilterAsync()
        {
            await FilterActChanged.InvokeAsync(FilterAct);
        }

        private async Task ClearFilterAsync()
        {
            FilterAct = new();
            FilterAct.OrgId = OrgId;
            await OnClickFilterAsync();
        }
    }
}
