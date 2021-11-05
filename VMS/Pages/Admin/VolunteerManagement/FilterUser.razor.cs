using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.Admin.VolunteerManagement
{
    public partial class FilterUser
    {
        private string yearChoosenValue = "Khóa";
        private bool isYearShow;
        private bool isYearGrey = false;

        private string rankChoosenValue = "Hạng";
        private bool isRankShow;
        private bool isRankGrey = false;

        private string labelChoosenValue = "Danh hiệu";
        private bool isLabelShow;
        private bool isLabelGrey = false;

        [CascadingParameter]
        public IModalService Modal { get; set; }

        private List<AreaViewModel> areas = new();
        private List<SkillViewModel> skills = new();
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

        private async Task ShowSkillsPopupAsync()
        {
            var skillsParameter = new ModalParameters();
            skillsParameter.Add("ChoosenSkillsList", skills);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<VMS.Pages.ActivitySearchPage.SkillsPopup>("", skillsParameter, options).Result;
        }
        public class fakeYears
        {
            public string Year { get; set; }
        }

        private List<fakeYears> Years = new()
        {
            new fakeYears() { Year = "Khóa 44" },
            new fakeYears() { Year = "Khóa 45" },
            new fakeYears() { Year = "Khóa 46" },
            new fakeYears() { Year = "Khóa 47" },
        };
        private void ChooseYearlValue(fakeYears year)
        {
            yearChoosenValue = year.Year;
            isYearGrey = true;
        }
        private void ToggleYearDropdown()
        {
            isYearShow = !isYearShow;
        }

        private void CloseCityDropdown()
        {
            isYearShow = false;
        }

        public class fakeRanks
        {
            public string Rank { get; set; }
        }

        private List<fakeRanks> Ranks = new()
        {
            new fakeRanks() { Rank = "Cao đến thấp" },
            new fakeRanks() { Rank = "Thấp đến cao" },
        };
        private void ChooseRanklValue(fakeRanks rank)
        {
            rankChoosenValue = rank.Rank;
            isRankGrey = true;
        }
        private void ToggleRankDropdown()
        {
            isRankShow = !isRankShow;
        }

        private void CloseRankDropdown()
        {
            isRankShow = false;
        }

        public class fakeLabels
        {
            public string Label { get; set; }
        }

        private List<fakeLabels> Labels = new()
        {
            new fakeLabels() { Label = "Siêu cấp đẹp trai" },
            new fakeLabels() { Label = "Siêu cấp đẹp gái" },
            new fakeLabels() { Label = "Siêu cấp cute" },
            new fakeLabels() { Label = "Đỗ nghèo khỉ" },
            new fakeLabels() { Label = "Thánh FA" },
            new fakeLabels() { Label = "Kẻ hủy diệt deadline" },
            new fakeLabels() { Label = "Chúa tể thả thính" },
        };
        private void ChooseLabellValue(fakeLabels label)
        {
            labelChoosenValue = label.Label;
            isLabelGrey = true;
        }
        private void ToggleLabelDropdown()
        {
            isLabelShow = !isLabelShow;
        }

        private void CloseLabelDropdown()
        {
            isLabelShow = false;
        }
        private async Task UpdateFilterValueAsync()
        {

        }
        private void ClearFilter()
        {
            yearChoosenValue = "Khóa";
            rankChoosenValue = "Hạng";
            labelChoosenValue = "Danh hiệu";
            isYearGrey = false;
            isRankGrey = false;
            isLabelGrey = false;
            areas = new();
        }
    }
}
