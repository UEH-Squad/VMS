using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using VMS.Application.ViewModels;
using VMS.Common.Extensions;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class Index : ComponentBase
    {
        private string searchValue = string.Empty;
        private FilterActivityViewModel filter = new();
        private bool[] orderList = new bool[3];
        private bool isSearch = false;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            HandleQueryString();
        }

        private void HandleQueryString()
        {
            if (NavigationManager.TryGetQueryString<bool>("newest", out var isNewest))
            {
                orderList[0] = isNewest;
            }

            if (NavigationManager.TryGetQueryString<bool>("nearest", out var isNearest))
            {
                orderList[1] = isNearest;
            }

            if (NavigationManager.TryGetQueryString<bool>("hottest", out var isHottest))
            {
                orderList[2] = isHottest;
            }

            if (NavigationManager.TryGetQueryString<List<int>>("area", out var listAreaId))
            {
                filter.Areas.AddRange(listAreaId);
            }
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
            isSearch = true;
        }

        private void FilterChanged(FilterActivityViewModel filter)
        {
            this.filter = filter;
            isSearch = false;
        }
    }
}
