using VMS.Common;
using Blazored.Modal;
using VMS.Common.Enums;
using VMS.Common.Extensions;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using System.Collections.Generic;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountUser : ComponentBase
    {
        private bool isCourseShow;
        private List<string> courses;
        private CreateAccountViewModel account = new() { Course = "Khóa" };

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ResultModal { get; set; }

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

            bool isSuccess = await AdminService.AddSingleAccountAsync(account, Role.User);

            await CloseModalAsync();

            ModalParameters parameters = new();
            parameters.Add("IsSuccess", isSuccess);
            ResultModal.Show<CreateFailed>("", parameters, BlazoredModalOptions.GetModalOptions());
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
