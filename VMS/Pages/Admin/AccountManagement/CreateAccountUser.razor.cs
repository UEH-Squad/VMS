using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountUser : ComponentBase
    {
        private bool isCourseShow;
        private bool? isSuccess;
        private List<string> courses;
        private CreateAccountViewModel account = new();

        protected override async Task OnInitializedAsync()
        {
            GenerateCourses();
        }

        private void GenerateCourses()
        {
            /* Tính khóa hiện tại bằng cách: [Năm hiện tại] - [Năm thành lập] + [Tháng hiện tại >= 9 ? 2 : 1] */
            int presentCourse = DateTime.Now.Year - 1976 + (DateTime.Now.Month >= 9 ? 2 : 1);

            courses = new();

            for (int i = 0; i < 8; i++)
            {
                courses.Add($"K{presentCourse - i}");
            }
        }

        private void ChooseCourseValue(string course)
        {
            account.Course = course;
        }

        private void ToggCourseDropdown()
        {
            isCourseShow = !isCourseShow;
        }

        private void CloseCourseDropdown()
        {
            isCourseShow = false;
        }

        private async Task OnValidSubmitAsync()
        {
            isSuccess = false;
        }
    }
}
