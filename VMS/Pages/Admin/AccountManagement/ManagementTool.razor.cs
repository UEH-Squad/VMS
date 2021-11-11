using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class ManagementTool : ComponentBase
    {
        private bool isCourseShow;
        private bool isLevelShow;
        private bool isShowDropDownCreate = false;

        private FilterAccountViewModel filter = new();
        private List<string> levels, courses;

        [Parameter] public Role PageRole { get; set; }
        [Parameter] public string Tilte { get; set; }
        [Parameter] public EventCallback<FilterAccountViewModel> FilterChanged { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

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

        private void ChooseLevelValue(string level)
        {
            filter.Course = level;
        }

        private void ToggLeLevelDropdown()
        {
            isLevelShow = !isLevelShow;
        }

        private void CloseLevelDropdown()
        {
            isLevelShow = false;
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
            filter.Course = string.Empty;
            await OnClickFilterAsync();
        }

        private void ShowDropDownCreate()
        {
            isShowDropDownCreate = !isShowDropDownCreate;
        }

        private void ShowCreateAccounts()
        {
            var parameters = new ModalParameters();
            parameters.Add("Role", PageRole);

            Modal.Show<CreateAccounts>("", parameters, BlazoredModalOptions.GetModalOptions());

            isShowDropDownCreate = false;
        }

        private void ShowCreateAccount()
        {
            switch (PageRole)
            {
                case Role.Admin:
                    Modal.Show<CreateAccountAdminSp>("", BlazoredModalOptions.GetModalOptions());
                    break;

                case Role.Organization:
                    Modal.Show<CreateAccountOrg>("", BlazoredModalOptions.GetModalOptions());
                    break;

                case Role.User:
                    Modal.Show<CreateAccountUser>("", BlazoredModalOptions.GetModalOptions());
                    break;
            }

            isShowDropDownCreate = false;
        }
    }
}
