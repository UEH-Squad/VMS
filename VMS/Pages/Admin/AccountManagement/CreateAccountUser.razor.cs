using System;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using VMS.Common;
using VMS.Common.Enums;
using VMS.Common.Extensions;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountUser : ComponentBase
    {
        private bool isCourseShow;
        private bool? isSuccess;
        private List<string> courses;
        private CreateAccountViewModel account = new();

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] private IAdminService AdminService { get; set; }

        protected override void OnInitialized()
        {
            courses = Courses.GetCourses();
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
            if (!IsValidAccount())
            {
                return;
            }

            isSuccess = await AdminService.AddSingleAccountAsync(account, Role.User);
        }

        private bool IsValidAccount()
        {
            return courses.Exists(x => x == account.Course) && account.IsValidAccount(Role.User);
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
