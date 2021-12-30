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
        private FilterVltViewModel filter = new();
        private List<FacultyViewModel> faculties = new();
        private string courseChoosenValue = "Khóa";
        private bool isCourseShow;
        private bool isCourseGrey = false;
        private string facultyChoosenValue = "Khoa";
        private bool isFacultyShow;
        private bool isFacultyGrey = false;

        [Parameter]
        public bool HaveSearchBox { get; set; }

        [Parameter]
        public bool IsLongDistanceFilter { get; set; }

        [Parameter]
        public EventCallback<FilterVltViewModel> FilterChanged { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        private IFacultyService FacultyService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            faculties = await FacultyService.GetAllFacultiesAsync();
        }

        private void ChooseCourseValue()
        {
        }

        private void ToggleCourseDropdown()
        {
            isCourseShow = !isCourseShow;
        }

        private void CloseCourseDropdown()
        {
            isCourseShow = false;
        }

        private void ChooseFacultyValue(FacultyViewModel faculty)
        {
            facultyChoosenValue = faculty.Name;
            filter.FacultyName = faculty.Name;
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
            filter = new();
            facultyChoosenValue = "Khoa";
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
