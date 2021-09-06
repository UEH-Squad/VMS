using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Pages.OrganizationManagementPage
{
    public partial class ActivitiesCard : ComponentBase
    {
        //private PagedResult<ActivityViewModel> pagedResult = new() { Results = new() };

        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
    }
}
