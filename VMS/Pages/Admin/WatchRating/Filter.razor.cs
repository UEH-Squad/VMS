using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Admin.WatchRating
{
    public partial class Filter : ComponentBase
    {
        [Parameter]
        public FilterRecruitmentViewModel Filters { get; set; }
        [Parameter]
        public EventCallback<FilterRecruitmentViewModel> FilterChanged { get; set; }
        int typeRating = 0;
        List<bool> star = new()
        {
            false,
            false,
            false,
            false,
            false
        };
        private async Task UpdateFilterValueAsync()
        {
           Filters.IsUserRating = (typeRating == 1);
           Filters.IsOrgRating = (typeRating == 2);
           for(int i =1; i<=5; i++)
            {
                if (star[i-1] == true)
                    Filters.Ranks.Add(i);
            }
            await FilterChanged.InvokeAsync(Filters);
        }

        private async Task ClearFilterAsync()
        {
            typeRating = 0;
            star = new()
            {
                false,
                false,
                false,
                false,
                false
            };
            Filters = new();
            await FilterChanged.InvokeAsync(Filters);
        }
    }
}
