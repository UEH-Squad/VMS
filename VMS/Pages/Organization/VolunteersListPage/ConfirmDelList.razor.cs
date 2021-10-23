using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class ConfirmDelList : ComponentBase
    {
        [Parameter]
        public List<int> CheckedList { get; set; }
        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }
        [Inject]
        private IListVolunteerService ListVolunteerService { get; set; }

        bool isConfirmDelete = true;
        bool isDeleteSuccess;

        private async Task CloseModal()
        {
            await Modal.CloseAsync(ModalResult.Ok<bool>(isDeleteSuccess));
        }

        private async Task DeleteSuccess()
        {
            isConfirmDelete = !isConfirmDelete;
            foreach (var item in CheckedList)
            {
                await ListVolunteerService.UpdateVounteer(item, true);
            }
            isDeleteSuccess = true;
            await CloseModal();
        }
    }
}
