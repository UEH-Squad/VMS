using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.SearchOrganizer
{
    public partial class Index : ComponentBase
    {
        private FilterOrgViewModel filter = new();

        private void OnFilterChanged(FilterOrgViewModel filter)
        {
            this.filter = filter;
        }
    }
}
