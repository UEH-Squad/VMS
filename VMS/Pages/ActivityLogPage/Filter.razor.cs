using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;

namespace VMS.Pages.ActivityLogPage
{
    public partial class Filter : ComponentBase
    {
        private string orgDefault = "Tổ chức";
        private string semesterDefault = "Học kỳ";
        private List<UserViewModel> organizers;
        private FilterRecruitmentViewModel filter = new();

        private readonly Dictionary<Semester, string> semesters = new(
            new List<KeyValuePair<Semester, string>>() {
                new KeyValuePair<Semester, string>(Semester.First, "Học kỳ đầu"),
                new KeyValuePair<Semester, string>(Semester.Middle, "Học kỳ giữa"),
                new KeyValuePair<Semester, string>(Semester.Last, "Học kỳ cuối")
        });

        [Parameter] public EventCallback<FilterRecruitmentViewModel> FilterChanged { get; set; }

        [Inject] private IOrganizationService OrganizationService { get; set; }

        protected override void OnInitialized()
        {
            organizers = OrganizationService.GetAllOrganizers();
        }

        private void ChooseOrg(UserViewModel organizer)
        {
            filter.OrgId = organizer.Id;
            orgDefault = organizer.FullName;
            Display2 = "d-none";
        }

        private void ChooseSemester(Semester semester)
        {
            semesterDefault = semesters[semester];
            filter.Semester = semester;
            Display1 = "d-none";
        }

        private async Task CheckOrderAsync(bool value)
        {
            filter.IsRated = value;
            await UpdateFilterValueAsync();
        }

        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(filter);
        }

        private async Task ClearFilterAsync()
        {
            filter = new();
            orgDefault = "Tổ chức";
            semesterDefault = "Học kỳ";
            await UpdateFilterValueAsync();
        }
    }
}