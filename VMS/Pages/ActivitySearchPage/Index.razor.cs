using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VMS.Application.ViewModels;
using VMS.Common.Extensions;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class Index : ComponentBase
    {
        private string searchValue = string.Empty;
        private FilterActivityViewModel filter = new();
        private bool isSearch = false;
        private Dictionary<ActOrderBy, bool> orderList = new(new List<KeyValuePair<ActOrderBy, bool>>() {
            new KeyValuePair<ActOrderBy, bool>(ActOrderBy.Newest, false),
            new KeyValuePair<ActOrderBy, bool>(ActOrderBy.Nearest, false),
            new KeyValuePair<ActOrderBy, bool>(ActOrderBy.Hottest, false)
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
                orderList[ActOrderBy.Newest] = isNewest;
            }

            if (NavigationManager.TryGetQueryString<bool>("nearest", out var isNearest))
            {
                orderList[ActOrderBy.Nearest] = isNearest;
            }

            if (NavigationManager.TryGetQueryString<bool>("hottest", out var isHottest))
            {
                orderList[ActOrderBy.Hottest] = isHottest;
            }

            if (NavigationManager.TryGetQueryString<List<int>>("area", out var listAreaId))
            {
                filter.Areas.AddRange(listAreaId.Select(a => new AreaViewModel()
                {
                    Id = a
                }));
            }
        }

        private void SearchValueChanged(string searchValue)
        {
            this.searchValue = searchValue;
            isSearch = !string.IsNullOrEmpty(searchValue);
        }

        private void FilterChanged(FilterActivityViewModel filter)
        {
            this.filter = filter;
            isSearch = false;
        }
    }
}
