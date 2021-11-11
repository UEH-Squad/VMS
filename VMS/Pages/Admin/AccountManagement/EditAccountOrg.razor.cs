using VMS.Common;
using Blazored.Modal;
using VMS.Common.Enums;
using VMS.Common.Extensions;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using VMS.Application.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using VMS.Application.Interfaces;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class EditAccountOrg : ComponentBase
    {
        private bool isConfirmShow;
        private bool isCourseShow;
        private string adminPassword;
        private List<string> courses;
        private AccountViewModel account = new();

        [Parameter] public AccountViewModel Account { get; set; }

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ModalConfirm { get; set; }

        [Inject] private IIdentityService IdentityService { get; set; }

        protected override void OnInitialized()
        {
            courses = Courses.GetLevels();
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
            parameters.Add("AccountRole", Role.Organization);

            var result = await ModalConfirm.Show<EditConfirm>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
            if ((bool)result.Data)
            {
                Account.Copy(account);
            }

            await Modal.CloseAsync();
        }

        private bool IsValidAccount()
        {
            return courses.Exists(x => x == account.Course)
                && account.IsValidAccount(Role.Organization)
                && IdentityService.IsCorrectCurrentUserPassword(adminPassword);
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
