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
        public FilterActivityViewModel FilterAct { get; set; }
        [Parameter]
        public EventCallback<FilterActivityViewModel> FilterActChanged { get; set; } 

        private async Task OnClickFilterAsync()
        {
            await FilterActChanged.InvokeAsync(FilterAct);
        }

        private async Task ClearFilter()
        {
            FilterAct = new();
            FilterAct.OrgId = OrgId;
            await OnClickFilterAsync();
        }
    }
}
