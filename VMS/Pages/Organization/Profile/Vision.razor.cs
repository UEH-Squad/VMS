using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.Profile
{
    public partial class Vision : ComponentBase
    {
        [Parameter]
        public UserViewModel Org { get; set; }
    }
}