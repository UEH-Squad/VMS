using Microsoft.AspNetCore.Components;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class Title : ComponentBase
    {
        [Parameter]
        public string Name { get; set; }
    }
}
