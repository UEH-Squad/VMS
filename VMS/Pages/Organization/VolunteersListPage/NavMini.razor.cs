using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;
using System.Collections.Generic;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class NavMini : ComponentBase
    {
        private int page = 1;
        private bool navDel = true; //hien thi mac dinh
        private bool navUndo = false; // mac dinh ko hien thi
        private string searchValue = string.Empty;
        [Parameter]
        public List<int> CheckedList { get; set; }
        [Parameter]
        public PaginatedList<ListVolunteerViewModel> PagedResult { get; set; }
        [Parameter]
        public int ActId { get; set; }
        [Parameter]
        public bool ShowDeletedList { get; set; }
        [Parameter]
        public EventCallback<PaginatedList<ListVolunteerViewModel>> OnChangeList { get; set; }
        [Parameter]
        public EventCallback<PaginatedList<ListVolunteerViewModel>> OnSearch { get; set; }
        [Parameter]
        public EventCallback<string> ValueChange { get; set; }
        [Inject]
        private IListVolunteerService ListVolunteerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, ShowDeletedList, page);
            await OnChangeList.InvokeAsync(PagedResult);
        }
        public async Task ChangeNav()
        {
            navDel = !navDel;
            navUndo = !navUndo;
            PagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, ShowDeletedList, page);
            await OnChangeList.InvokeAsync(PagedResult);
            await ValueChange.InvokeAsync(searchValue);
        }
        private async Task SearchValueChanged(string searchValueChanged)
        {
            this.searchValue = searchValueChanged;
            PagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, !ShowDeletedList, page);
            await OnSearch.InvokeAsync(PagedResult);
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
                    PagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, false, page);
                    await OnChangeList.InvokeAsync(PagedResult);
                }
            }
            else
            {
               var result = await Modal.Show<ConfirmUndoList>("", parameters, options).Result;
                if ((bool)result.Data)
                {
                    PagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, true, page);
                    await OnChangeList.InvokeAsync(PagedResult);
                }
            }
            await OnChangeList.InvokeAsync(PagedResult);

        }
    }
}
