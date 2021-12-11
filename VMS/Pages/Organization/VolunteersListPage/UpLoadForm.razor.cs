using VMS.Common;
using Blazored.Modal;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class UpLoadForm : ComponentBase
    {
        private IBrowserFile file;
        private readonly string acceptPattern = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        [Parameter]
        public int ActId { get; set; }
        private bool isSuccess = false;
        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ResultModal { get; set; }
        [Inject] private IExportExcelService ExportExcelService { get; set; }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync(ModalResult.Ok<bool>(isSuccess));
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
        private async Task OnClickUploadAsync()
        {
            isSuccess = await ExportExcelService.UpdateListVolunteerFromExcelFileAsync(file, ActId);
            ModalParameters parameters = new();
            parameters.Add("IsSuccess", isSuccess);
            ResultModal.Show<UploadSuccess>("", parameters, BlazoredModalOptions.GetModalOptions());
            await CloseModalAsync();
        }
    }
}
