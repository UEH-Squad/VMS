using VMS.Common;
using Blazored.Modal;
using VMS.Common.Enums;
using VMS.Common.Extensions;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountAdminSp : ComponentBase
    {
        private CreateAccountViewModel account = new();

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ResultModal { get; set; }

        [Inject] private IAdminService AdminService { get; set; }

        private async Task OnValidSubmitAsync()
        {
            if (!account.IsValidAccount(Role.Admin))
            {
                return;
            }

            bool isSuccess = await AdminService.AddSingleAccountAsync(account, Role.Admin);

            await CloseModalAsync();

            ModalParameters parameters = new();
            parameters.Add("IsSuccess", isSuccess);
            ResultModal.Show<CreateFailed>("", parameters, BlazoredModalOptions.GetModalOptions());
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
