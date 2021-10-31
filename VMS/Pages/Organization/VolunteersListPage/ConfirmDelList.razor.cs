using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class ConfirmDelList : ComponentBase
    {
        [Parameter]
        public bool Undo { get; set; }
        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }
        bool isConfirm = true;
        bool isSuccess;
        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync(ModalResult.Ok<bool>(isSuccess));
        }
        private async Task ActionSuccessAsync()
        {
            isConfirm = !isConfirm;
            isSuccess = true;
            await CloseModalAsync();
        }
    }
}
