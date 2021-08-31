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

        [Parameter]
        public string OrderByUrl { get; set; }
        [Parameter]
        public string AreaIdByUrl { get; set; }

        protected override void OnInitialized()
        {
            switch (OrderByUrl?.ToLower())
            {
                case "newest":
                    orderList[0] = true;
                    orderList[1] = true;
                    break;
                case "hottest":
                    orderList[2] = true;
                    break;
            }

            if (!string.IsNullOrEmpty(AreaIdByUrl))
            {
                if (int.TryParse(AreaIdByUrl, out int areaId))
                {
                    filter.Areas.Add(areaId);
                }
            }
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
            isSearch = true;
        }
    }
}
