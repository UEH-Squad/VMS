using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Common.Extensions;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class Index : ComponentBase
    {
        private FilterActivityViewModel filter = new();

        private void SearchValueChanged(string searchValue)
        {
            filter.SearchValue = searchValue;
            filter.IsSearch = true;
        }

        private void FilterChanged(FilterActivityViewModel filter)
        {
            this.filter = filter;
            this.filter.IsSearch = false;
        }
    }
}
