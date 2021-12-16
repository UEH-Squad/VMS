using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityLogPage
{
    public partial class Index : ComponentBase
    {
        private FilterRecruitmentViewModel filterChange = new();

        private void SearchValueChanged(string value)
        {
            filterChange.SearchValue = value;
            filterChange.IsSearch = true;
        }

        private void FilterChanged(FilterRecruitmentViewModel value)
        {
            filterChange = value;
            filterChange.IsSearch = false;
        }
    }
}
