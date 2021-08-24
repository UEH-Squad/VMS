using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class Index : ComponentBase
    {
        private string searchValue = string.Empty;
        private FilterActivityViewModel filter = new();

        private void FilterValueChanged(FilterActivityViewModel filter)
        {
            this.filter = filter;
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
        }
    }
}
