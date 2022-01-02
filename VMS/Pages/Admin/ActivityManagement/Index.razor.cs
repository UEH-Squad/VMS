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
