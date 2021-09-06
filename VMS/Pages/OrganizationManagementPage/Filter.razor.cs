using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.OrganizationManagementPage
{
    public partial class Filter : ComponentBase
    {
        [Parameter]
        public FilterActivityViewModel FilterAct { get; set; }
        [Parameter]
        public EventCallback<FilterActivityViewModel> FilterActChanged { get; set; } 
    }
}
