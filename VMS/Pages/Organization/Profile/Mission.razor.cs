using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.Organization.Profile
{
    public partial class Mission : ComponentBase
    {
        [Parameter]
        public UserViewModel Org { get; set; }
    }
}