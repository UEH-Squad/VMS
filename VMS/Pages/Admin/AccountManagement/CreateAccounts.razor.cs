using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccounts : ComponentBase
    {
        private readonly string acceptPattern = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private IBrowserFile file;
        private bool? isSuccess;

        [Parameter] public Role Role { get; set; } = Role.User;

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] private IExcelService ExcelService { get; set; }
        [Inject] private IIdentityService IdentityService { get; set; }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }

        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            if (e.File.ContentType != acceptPattern)
            {
                return;
            }
            else
            {
                file = e.File;
            }
        }

        private async Task OnClickCreateAsync()
        {
            List<CreateAccountViewModel> accounts = await ExcelService.GetListAccountFromExcelFileAsync(file);

            if (accounts == null)
            {
                isSuccess = false;
            }
            else
            {
                IdentityService.AddListAccount(accounts, Role);
                isSuccess = true;
            }
        }
    }
}
