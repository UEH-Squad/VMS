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

        private void ChooseSemester(string semester)
        {
            semesterDefault = semester;
            Display1 = "d-none";
        }

        private void ChooseFaculty(FacultyViewModel faculty)
        {
            facultyDefault = faculty.Name;
            Display2 = "d-none";
        }

        private async Task RadioButtonChanged(bool value)
        {
            await IsRatedChanged.InvokeAsync(value);
        }

    }
}