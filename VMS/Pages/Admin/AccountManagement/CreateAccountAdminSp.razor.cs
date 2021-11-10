using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Common.Extensions;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountAdminSp : ComponentBase
    {
        private bool? isSuccess;
        private CreateAccountViewModel account = new();

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] private IAdminService AdminService { get; set; }

        private async Task OnValidSubmitAsync()
        {
            if (!account.IsValidAccount(Role.Admin))
            {
                return;
            }

            isSuccess = await AdminService.AddSingleAccountAsync(account, Role.Admin);
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
