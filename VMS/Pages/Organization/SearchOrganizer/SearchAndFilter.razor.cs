using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.Organization.SearchOrganizer
{
    public partial class SearchAndFilter
    {
        private string levelChoosenValue = "Cấp";
        private bool isLevelShow;
        private bool isLevelGrey = false;

        [CascadingParameter]
        public IModalService Modal { get; set; }

        private List<AreaViewModel> areas = new();

        public class fakeLevels
        {
            public string Name { get; set; }
        }

        private List<fakeLevels> levels = new()
        {
            new fakeLevels() { Name = "Ban Chuyên môn" },
            new fakeLevels() { Name = "Khoa/Viện/KTX" },
            new fakeLevels() { Name = "CLB/Đội/Nhóm" },
        };

        private void ChooseLevelValue(fakeLevels level)
        {
            levelChoosenValue = level.Name;
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
            parameters.Add("ChoosenAreasList", areas);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<VMS.Pages.ActivitySearchPage.AreasPopup>("", parameters, options).Result;
        }
        private async Task UpdateFilterValueAsync()
        {

        }
        private void ClearFilter()
        {
            levelChoosenValue = "Cấp";
            isLevelGrey = false;
            areas = new();
        }
    }
}
