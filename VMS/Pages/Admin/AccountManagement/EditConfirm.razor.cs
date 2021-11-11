using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class EditConfirm : ComponentBase
    {
        private bool? isSuccess;

        [Parameter] public AccountViewModel Account { get; set; }
        [Parameter] public Role AccountRole { get; set; }

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] IAdminService AdminService { get; set; }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync(ModalResult.Ok(isSuccess));
        }

        private async Task ShowEditSuccessAsync()
        {
            isSuccess = await AdminService.UpdateAccountAsync(Account, AccountRole);
        }
    }
}
