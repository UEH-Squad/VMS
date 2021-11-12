using VMS.Common;
using Blazored.Modal;
using VMS.Common.Enums;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccounts : ComponentBase
    {
        private IBrowserFile file;
        private readonly string acceptPattern = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        [Parameter] public Role Role { get; set; } = Role.User;

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ResultModal { get; set; }

        [Inject] private IExcelService ExcelService { get; set; }

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
            bool isSuccess = await ExcelService.AddListAccountsFromExcelFileAsync(file, Role);

            await CloseModalAsync();

            ModalParameters parameters = new();
            parameters.Add("IsSuccess", isSuccess);
            ResultModal.Show<CreateFailed>("", parameters, BlazoredModalOptions.GetModalOptions());
        }
    }
}
