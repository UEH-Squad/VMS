using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivityLogPage
{
    public partial class Filter
    {
        [Inject]
        private IOrganizationService OrganizationService { get; set; }

        [Parameter]
        public EventCallback<bool?> IsRatedChanged { get; set; }
        [Parameter]
        public EventCallback<FilterRecruitmentViewModel> FilterChanged { get; set; }
        [Parameter]
        public FilterRecruitmentViewModel FilterChange { get; set; } = new();

        private string orgDefault = "Tổ chức";
        private string semesterDefault = "Học kỳ";
        private List<UserViewModel> organizers;

        private readonly List<string> semesters = new()
        {
            "Học kỳ đầu",
            "Học kỳ giữa",
            "Học kỳ cuối"
        };

        protected override async Task OnInitializedAsync()
        {
            organizers = OrganizationService.GetAllOrganizers();
        }

        private void ChooseFacultyAsync(UserViewModel organizer)
        {
            FilterChange.OrgId = organizer.Id;
            orgDefault = organizer.FullName;
            Display2 = "d-none";
        }

        private void ChooseSemesterAsync(string semester)
        {
            semesterDefault = semester;
            FilterChange.Semester = semesterDefault;
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
            orgDefault = "Tổ chức";
            semesterDefault = "Học kỳ";
            FilterChange = new FilterRecruitmentViewModel();
            await FilterChanged.InvokeAsync(FilterChange);
            await IsRatedChanged.InvokeAsync(new bool?());
        }

    }
}