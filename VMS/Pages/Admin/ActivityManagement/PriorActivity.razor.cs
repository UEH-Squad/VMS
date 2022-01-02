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
        [Parameter]
        public List<ActivityViewModel> ListPinned { get; set; }
        private List<int> unPinList = new();
        private bool isSuccess = true;

        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        private async Task CloseModal()
        {
            await Modal.CloseAsync();
        }
        private async Task HandleCheckAsync(int id)
        {
            var checkItem = ListPinned.Find(x => x.Id == id);
            if (checkItem is not null)
            {
                checkItem.IsCheck = !checkItem.IsCheck;
                if (checkItem.IsCheck == true)
                {
                    unPinList.Add(id);
                }
                else
                {
                    unPinList.Remove(id);
                }
            }
        }
        private async Task Success()
        {
            isSuccess = !isSuccess;
            await Modal.CloseAsync(ModalResult.Ok(unPinList));
        }
    }
}
