using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using VMS.Common.Extensions;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class Index : ComponentBase
    {
        private string searchValue = string.Empty;
        private FilterActivityViewModel filter = new();
        private bool[] orderList = new bool[3];
        private bool isSearch;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            if (NavigationManager.TryGetQueryString<string>("order", out var orderByUrl))

            switch (orderByUrl?.ToLower())
            {
                case "newest":
                    orderList[0] = true;
                    orderList[1] = true;
                    break;
                case "hottest":
                    orderList[2] = true;
                    break;
            }

            //if (!string.IsNullOrEmpty(AreaIdByUrl))
            //{
            //    if (int.TryParse(AreaIdByUrl, out int areaId))
            //    {
            //        filter.Areas.Add(areaId);
            //    }
            //}
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
            isSearch = true;
        }
    }
}
