using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;
using VMS.Common.Extensions;

namespace VMS.Pages.Admin.AccountManagement
{
    public  partial class EditAccountUser : ComponentBase
    {
        private bool isConfirmShow;
        private bool isCourseShow;
        private List<string> courses;
        private AccountViewModel account = new();

        [Parameter] public AccountViewModel Account { get; set; }

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ModalConfirm { get; set; }

        protected override void OnInitialized()
        {
            courses = Courses.GetCourses();
            account.Copy(Account);
        }

        private void ChooseCourseValue(string course)
        {
            account.Course = course;
        }

        private void ToggCourseDropdown()
        {
            isCourseShow = !isCourseShow;
        }

        private void CloseLevelDropdown()
        {
            isCourseShow = false;
        }

        private async Task OnValidSubmitAsync()
        {
            if (!IsValidAccount())
            {
                return;
            }

            isConfirmShow = true;

            ModalParameters parameters = new();
            parameters.Add("Account", account);
            parameters.Add("AccountRole", Role.User);

            var result = await ModalConfirm.Show<EditConfirm>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
            var isSuccess = (bool?)result.Data;
            if (isSuccess.HasValue && isSuccess.Value)
            {
                Account.Copy(account);
            }

            await Modal.CloseAsync();
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
