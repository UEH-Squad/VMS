using VMS.Common;
using Blazored.Modal;
using VMS.Common.Enums;
using VMS.Common.Extensions;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class EditAccountAdminSp : ComponentBase
    {
        private bool isConfirmShow;
        private string adminPassword;
        private AccountViewModel account = new();

        [Parameter] public AccountViewModel Account { get; set; }

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ModalConfirm { get; set; }

        [Inject] private IIdentityService IdentityService { get; set; }

        protected override void OnInitialized()
        {
            account.Copy(Account);
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
            parameters.Add("AccountRole", Role.Admin);

            var result = await ModalConfirm.Show<EditConfirm>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
            if ((bool)result.Data)
            {
                Account.Copy(account);
            }

            await Modal.CloseAsync();
        }

        private bool IsValidAccount()
        {
            return account.IsValidAccount(Role.Admin)
                && IdentityService.IsCorrectCurrentUserPassword(adminPassword);
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
