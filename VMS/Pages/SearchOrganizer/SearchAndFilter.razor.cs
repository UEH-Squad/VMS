using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.SearchOrganizer
{
    public partial class SearchAndFilter
    {
        private string levelChoosenValue = "Cấp";
        private bool isLevelShow;
        private bool isLevelGrey = false;
        private FilterOrgViewModel filter = new();
        private List<string> levels;

        [Parameter]
        public EventCallback<FilterOrgViewModel> FilterChanged { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        protected override void OnInitialized()
        {
            levels = Courses.GetLevels();
        }

        private void ChooseLevelValue(string level)
        {
            filter.Course = level;
            levelChoosenValue = level;
            isLevelGrey = true;
        }

        private void ToggleLevelDropdown()
        {
            isLevelShow = !isLevelShow;
        }

        private void CloseLevelDropdown()
        {
            isLevelShow = false;
        }

        private async Task ShowAreasPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", filter.Areas);

            await Modal.Show<ActivitySearchPage.AreasPopup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }
        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(filter);
        }

        private async Task ClearFilterAsync()
        {
            levelChoosenValue = "Cấp";
            isLevelGrey = false;
            filter = new();

            await UpdateFilterValueAsync();
        }

        private async Task OnSearchValueChangedAsync(string searchValue)
        {
            filter.SearchValue = searchValue;
            filter.IsSearch = true;

            await UpdateFilterValueAsync();
        }
    }
}
