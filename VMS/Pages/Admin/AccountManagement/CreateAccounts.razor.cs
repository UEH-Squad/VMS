using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccounts : ComponentBase
    {
        private string acceptPattern = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        [Inject] private IExcelService ExcelService { get; set; }

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            await ExcelService.GetListAccountFromExcelFileAsync(e.File);
        }
    }
}
