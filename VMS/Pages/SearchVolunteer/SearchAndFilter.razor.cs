using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.SearchVolunteer
{
    public partial class SearchAndFilter : ComponentBase
    {
        private FilterVolunteerViewModel filter = new();
        private List<FacultyViewModel> faculties = new();
        private string courseChoosenValue = "Khóa";
        private bool isCourseShow;
        private bool isCourseGrey = false;
        private List<string> courses;
        private string facultyChoosenValue = "Khoa";
        private bool isFacultyShow;
        private bool isFacultyGrey = false;

        [Parameter]
        public bool HaveSearchBox { get; set; }

        [Parameter]
        public bool IsLongDistanceFilter { get; set; }

        [Parameter]
        public EventCallback<FilterVolunteerViewModel> FilterChanged { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        private IFacultyService FacultyService { get; set; }

        protected override void OnInitialized()
        {
            courses = Courses.GetCourses();
        }

        private void ChooseCourseValue(string course)
        {
            filter.Course = course;
            courseChoosenValue = course;
            isCourseGrey = true;
        }

        private void ToggleCourseDropdown()
        {
            isCourseShow = !isCourseShow;
        }

        private void CloseCourseDropdown()
        {
            isCourseShow = false;
        }

        protected override async Task OnInitializedAsync()
        {
            faculties = await FacultyService.GetAllFacultiesAsync();
        }

        private void ChooseFacultyValue(FacultyViewModel faculty)
        {
            filter.FacultyName = faculty.Name;
            facultyChoosenValue = faculty.Name;
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

        private async Task ShowAreasPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", filter.Areas);

            await Modal.Show<ActivitySearchPage.AreasPopup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowSkillsPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenSkillsList", filter.Skills);

            await Modal.Show<ActivitySearchPage.SkillsPopup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(filter);
        }

        private async Task ClearFilterAsync()
        {
            courseChoosenValue = "Khóa";
            facultyChoosenValue = "Khoa";
            filter = new();
            isCourseGrey = false;
            isFacultyGrey = false;
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
