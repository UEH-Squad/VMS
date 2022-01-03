using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common;

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

        private List<string> courses;
        private List<AreaViewModel> areas = new();
        private List<SkillViewModel> skills = new();

        [CascadingParameter]
        public IModalService Modal { get; set; }

        protected override void OnInitialized()
        {
            courses = Courses.GetCourses();
        }

        private async Task ShowAreasPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", areas);

            await Modal.Show<ActivitySearchPage.AreasPopup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowSkillsPopupAsync()
        {
            var skillsParameter = new ModalParameters();
            skillsParameter.Add("ChoosenSkillsList", skills);

            await Modal.Show<ActivitySearchPage.SkillsPopup>("", skillsParameter, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private void ChooseYearValue(string year)
        {
            yearChoosenValue = year;
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
            skills = new();
        }
    }
}
