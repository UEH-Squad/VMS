﻿using Blazored.Modal;
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

        private FilterAccountViewModel filter = new();
        private List<string> courses;

        [Parameter] public Role PageRole { get; set; }
        [Parameter] public string Tilte { get; set; }
        [Parameter] public EventCallback<FilterAccountViewModel> FilterChanged { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        protected override void OnInitialized()
        {
            courses = PageRole == Role.User ? Courses.GetCourses() : Courses.GetLevels();
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

        private string GetDropdownValue()
        {
            return !string.IsNullOrEmpty(filter.Course) ? filter.Course
                : PageRole == Role.User ? "Khóa" : "Cấp";
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

        private async Task ShowCreateAccountsAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("Role", PageRole);

            await Modal.Show<CreateAccounts>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            await ClearFilterAsync();
        }

        private async Task ShowCreateAccountAsync()
        {
            switch (PageRole)
            {
                case Role.Admin:
                    await Modal.Show<CreateAccountAdminSp>("", BlazoredModalOptions.GetModalOptions()).Result;
                    break;

                case Role.Organization:
                    await Modal.Show<CreateAccountOrg>("", BlazoredModalOptions.GetModalOptions()).Result;
                    break;

                case Role.User:
                    await Modal.Show<CreateAccountUser>("", BlazoredModalOptions.GetModalOptions()).Result;
                    break;
            }

            await ClearFilterAsync();
        }
    }
}
