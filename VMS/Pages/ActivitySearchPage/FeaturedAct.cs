using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class FeaturedAct : ComponentBase
    {
        [Parameter]
        public List<ActivityViewModel> FeaturedActivities { get; set; }
    }
}
