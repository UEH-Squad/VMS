using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class Index : ComponentBase
    {
        private string searchValue = string.Empty;
        private FilterActivityViewModel filter = new();
        private bool[] orderList = new bool[3];
        private bool isSearch;

        private void FilterChanged(FilterActivityViewModel filter)
        {
            this.filter = filter;
            isSearch = false;
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
            isSearch = true;
        }
    }
}
