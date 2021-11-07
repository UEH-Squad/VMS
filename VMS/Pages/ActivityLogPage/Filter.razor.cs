using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivityLogPage
{
    public partial class Filter
    {
        [Inject]
        private IFacultyService FacultyService { get; set; }

        [Parameter]
        public EventCallback<bool?> IsRatedChanged { get; set; }
        [Parameter]
        public EventCallback<FilterRecruitmentViewModel> FilterChanged { get; set; }
        [Parameter]
        public FilterRecruitmentViewModel FilterChange { get; set; } = new();

        private string facultyDefault = "Đơn vị";
        private string semesterDefault = "Học kỳ";
        private List<FacultyViewModel> faculties = new();

        private readonly List<string> semesters = new()
        {
            "Học kỳ đầu",
            "Học kỳ giữa",
            "Học kỳ cuối"
        };

        protected override async Task OnInitializedAsync()
        {
            faculties = await FacultyService.GetAllFacultiesAsync();
        }
     
        private void ChooseFaculty(FacultyViewModel faculty)
        {
            facultyDefault = faculty.Name;
            FilterChange.FullName = facultyDefault;
            Display2 = "d-none";
        }

        private void ChooseSemester(string semester)
        {
            semesterDefault = semester;
            Display1 = "d-none";
        }

        private async Task RadioButtonChangedAsync(bool value)
        {
            await IsRatedChanged.InvokeAsync(value);
        }

        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(FilterChange);
        }

        private async Task ClearFilter()
        {
            facultyDefault = "Đơn vị";
            semesterDefault = "Học kỳ";
            FilterChange = new FilterRecruitmentViewModel();
            await FilterChanged.InvokeAsync(FilterChange);
        }

    }
}