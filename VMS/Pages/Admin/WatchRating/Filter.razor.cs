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
        private FilterRecruitmentViewModel filter = new();

        [Parameter]
        public EventCallback<FilterRecruitmentViewModel> FilterChanged { get; set; }

        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(filter);
        }

        private async Task ClearFilterAsync()
        {
            filter = new();
            await UpdateFilterValueAsync();
        }

        private void OnRadioChanged(bool value)
        {
            filter.IsOrgRating = value;
        }

        private void OnCheckBoxChanged(double value)
        {
            if (filter.Ranks.Exists(x => x == value))
            {
                filter.Ranks.Remove(value);
            }
            else
            {
                filter.Ranks.Add(value);
            }
        }
    }
}
