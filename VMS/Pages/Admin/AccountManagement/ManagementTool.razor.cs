using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class ManagementTool : ComponentBase
    {
        private string courseChoosenValue = "Khóa";
        private bool isCourseShow;

        private FilterAccountViewModel filter = new();
        private List<string> levels, courses;

        [Parameter] public bool IsAccountOrg { get; set; }
        [Parameter] public bool IsAccountUser { get; set; }
        [Parameter] public bool IsAccountAdminSp { get; set; }
        [Parameter] public string Tilte { get; set; }
        [Parameter] public EventCallback<FilterAccountViewModel> FilterChanged { get; set; }

        protected override void OnInitialized()
        {
            levels = Courses.GetLevels();
            courses = Courses.GetCourses();
        }

        private void ChooseCourseValue(string course)
        {
            filter.Course = course;
        }

        private void ToggCourseDropdown()
        {
            isCourseShow = !isCourseShow;
        }

        private void CloseCourseDropdown()
        {
            isCourseShow = false;
        }

        private async Task OnClickFilterAsync()
        {
            await FilterChanged.InvokeAsync(filter);
        }

        private async Task OnClickOrderAsync(bool order)
        {
            filter.IsNewest = order;
            await OnClickFilterAsync();
        }

        private async Task ClearFilterAsync()
        {
            filter = new();
            await OnClickFilterAsync();
        }
    }
}
