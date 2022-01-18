using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Admin.VolunteerManagement
{
    public partial class FilterUser
    {
        private string yearChoosenValue = "Khóa";
        private bool isYearShow;
        private bool isYearGrey = false;

        private string facultyChoosenValue = "Khoa";
        private bool isFacultyShow;
        private bool isFacultyGrey = false;

        private string rankChoosenValue = "Hạng";
        private bool isRankShow;
        private bool isRankGrey = false;

        private string labelChoosenValue = "Danh hiệu";
        private bool isLabelShow;
        private bool isLabelGrey = false;

        private List<string> courses;
        private List<FacultyViewModel> faculties;

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Parameter]
        public FilterVolunteerViewModel Filter { get; set; } = new();

        [Parameter]
        public EventCallback<FilterVolunteerViewModel> FilterChanged { get; set; }

        [Inject]
        private IFacultyService FacultyService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            courses = Courses.GetCourses();
            faculties = await FacultyService.GetAllFacultiesAsync();
        }

        private async Task ShowAreasPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", Filter.Areas);

            await Modal.Show<ActivitySearchPage.AreasPopup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowSkillsPopupAsync()
        {
            var skillsParameter = new ModalParameters();
            skillsParameter.Add("ChoosenSkillsList", Filter.Skills);

            await Modal.Show<ActivitySearchPage.SkillsPopup>("", skillsParameter, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private void ChooseYearValue(string year)
        {
            yearChoosenValue = year;
            Filter.Course = year;
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

        private void ChooseFacultyValue(string faculty)
        {
            facultyChoosenValue = faculty;
            Filter.FacultyName = faculty;
            isFacultyGrey = true;
        }

        private void ToggleFacultyDropdown()
        {
            isFacultyShow = !isFacultyShow;
        }

        private void CloseFacultyDropdown()
        {
            isFacultyShow = false;
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
            await FilterChanged.InvokeAsync(Filter);
        }

        private async Task ClearFilterAsync()
        {
            yearChoosenValue = "Khóa";
            facultyChoosenValue = "Khoa";
            rankChoosenValue = "Hạng";
            labelChoosenValue = "Danh hiệu";
            isYearGrey = false;
            isFacultyGrey = false;
            isRankGrey = false;
            isLabelGrey = false;
            Filter = new();

            await UpdateFilterValueAsync();
        }
    }
}
