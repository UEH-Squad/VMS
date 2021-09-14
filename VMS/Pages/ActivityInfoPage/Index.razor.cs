using Microsoft.AspNetCore.Components;

namespace VMS.Pages.ActivityInfoPage
{
    public partial class Index : ComponentBase
    {
        [Parameter]
        public string ActivityId { get; set; }
    }
}