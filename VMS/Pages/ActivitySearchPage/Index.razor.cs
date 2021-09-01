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
        private bool isSearch = false;
        private Dictionary<string, bool> orderList = new(new List<KeyValuePair<string, bool>>() {
            new KeyValuePair<string, bool>("newest", false),
            new KeyValuePair<string, bool>("nearest", false),
            new KeyValuePair<string, bool>("hottest", false)
        });

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
                orderList["newest"] = isNewest;
            }

            if (NavigationManager.TryGetQueryString<bool>("nearest", out var isNearest))
            {
                orderList["nearest"] = isNearest;
            }

            if (NavigationManager.TryGetQueryString<bool>("hottest", out var isHottest))
            {
                orderList["hottest"] = isHottest;
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
