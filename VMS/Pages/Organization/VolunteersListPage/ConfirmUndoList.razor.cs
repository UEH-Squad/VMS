using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class ConfirmUndoList : ComponentBase
    {

        [Parameter]
        public List<int> CheckedList { get; set; }
        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        [Inject]
        private IListVolunteerService ListVolunteerService { get; set; }

        bool isConfirmUndo = true;
        bool isUndoSuccess;

        private async Task CloseModal()
        {
            await Modal.CloseAsync(ModalResult.Ok<bool>(isUndoSuccess));
        }

        private async Task UndoSuccess()
        {
            isConfirmUndo = !isConfirmUndo;
            foreach (var item in CheckedList)
            {
             await   ListVolunteerService.UpdateVounteer(item, false);
            }
               
            isUndoSuccess = true;
            await CloseModal();
        }
    }
}
