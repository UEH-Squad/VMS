using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;
using System.Collections.Generic;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class NavMini : ComponentBase
    {
        private bool navDel = true; //hien thi mac dinh
        private bool navUndo = false; // mac dinh ko hien thi
        private string searchValue = string.Empty;
        [Parameter]
        public long Quantity { get; set; }
        [Parameter]
        public int ActId { get; set; }
        [Parameter]
        public List<int> CheckedList { get; set; }
        [Parameter]
        public bool ShowDeletedList { get; set; }
        [Parameter]
        public EventCallback<string> ValueChange { get; set; }
        [Parameter]
        public EventCallback<bool> IsDeleted { get; set; }

        public async Task ChangeNav()
        {
            navDel = !navDel;
            navUndo = !navUndo;
            this.ShowDeletedList = !ShowDeletedList;
            await IsDeleted.InvokeAsync(ShowDeletedList);
        }
        private async Task SearchValueChanged(string searchValueChanged)
        {
            this.searchValue = searchValueChanged;
            await ValueChange.InvokeAsync(searchValue);
        }
        [CascadingParameter] public IModalService Modal { get; set; }
        async Task ShowConfirm(string action)
        {
            var parameters = new ModalParameters();
            parameters.Add("CheckedList", CheckedList);
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            if (action == "delete")
            {
                var result = await Modal.Show<ConfirmDelList>("", parameters, options).Result;
                if ((bool)result.Data)
                {
                    this.ShowDeletedList = false;
                    await IsDeleted.InvokeAsync(ShowDeletedList);
                }
            }
            else
            {
               var result = await Modal.Show<ConfirmUndoList>("", parameters, options).Result;
                if ((bool)result.Data)
                {
                    this.ShowDeletedList = true;
                    await IsDeleted.InvokeAsync(ShowDeletedList);
                }
            }

        }
    }
}
