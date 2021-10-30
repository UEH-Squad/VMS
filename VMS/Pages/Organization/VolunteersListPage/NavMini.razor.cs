using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using System.Collections.Generic;
using VMS.GenericRepository;
using VMS.Application.ViewModels;
using Microsoft.JSInterop;
using System;

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
        public PaginatedList<ListVolunteerViewModel> Data { get; set; }
        [Parameter]
        public EventCallback<string> ValueChange { get; set; }
        [Parameter]
        public EventCallback<bool> IsDeleted { get; set; }
        [Inject]
        private IExportExcelService ExportExcelService { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        public async Task ChangeNavAsync()
        {
            navDel = !navDel;
            navUndo = !navUndo;
            this.ShowDeletedList = !ShowDeletedList;
            await IsDeleted.InvokeAsync(ShowDeletedList);
        }
        private async Task SearchValueChangedAsync(string searchValueChanged)
        {
            this.searchValue = searchValueChanged;
            await ValueChange.InvokeAsync(searchValue);
        }
        [CascadingParameter] public IModalService Modal { get; set; }
        [Inject]
        private IRecruitmentService ListVolunteerService { get; set; }
        async Task ShowConfirmAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("Undo", !ShowDeletedList);
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
                var result = await Modal.Show<ConfirmDelList>("", parameters, options).Result;
                if ((bool)result.Data)
                {
                    await ListVolunteerService.UpdateVounteerAsync(CheckedList, !ShowDeletedList);
                    await IsDeleted.InvokeAsync(ShowDeletedList);
                }

        }
        public async Task DowLoadAsync()
        {
            await JSRuntime.InvokeVoidAsync("vms.SaveAs", "DSTNV_" + ActId + "_" + DateTime.Now.ToString()+".xlsx", ExportExcelService.ResultExportToExcel(Data, ActId));
        }
    }
}
