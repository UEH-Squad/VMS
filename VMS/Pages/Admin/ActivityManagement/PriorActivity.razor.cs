using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class PriorActivity : ComponentBase
    {
        private bool isSuccess = true;

        [Parameter] public List<ActivityViewModel> ListPinned { get; set; }

        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync(ModalResult.Ok(ListPinned));
        }

        private async Task SuccessAsync()
        {
            isSuccess = !isSuccess;
            await CloseModalAsync();
        }
    }
}
