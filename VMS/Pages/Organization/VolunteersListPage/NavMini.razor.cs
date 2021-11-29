using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using System.Collections.Generic;
using VMS.GenericRepository;
using VMS.Application.ViewModels;
using VMS.Common;

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
        public List<ListVolunteerViewModel> FullList { get; set; }
        [Parameter]
        public EventCallback<string> ValueChange { get; set; }
        [Parameter]
        public EventCallback<bool> ShowDelete { get; set; }
        [Parameter]
        public EventCallback<bool> HandleDeleted { get; set; }
        [Parameter]
        public EventCallback DowLoad { get; set; }
        [Parameter]
        public EventCallback Upload { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }
        [Inject]
        private IRecruitmentService ListVolunteerService { get; set; }
        public async Task ChangeNavAsync()
        {
            searchValue = string.Empty;
            navDel = !navDel;
            navUndo = !navUndo;
            this.ShowDeletedList = !ShowDeletedList;
            await ShowDelete.InvokeAsync(ShowDeletedList);
        }
        private async Task SearchValueChangedAsync(string searchValueChanged)
        {
            this.searchValue = searchValueChanged;
            await ValueChange.InvokeAsync(searchValue);
        }
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
            if((bool)result.Data)
            {
                await ListVolunteerService.UpdateVounteerAsync(CheckedList, !ShowDeletedList);
                await HandleDeleted.InvokeAsync();
            }
        }
        async void ShowUploadAsync()
        {
            var parameter = new ModalParameters();
            parameter.Add("ActId", ActId);
            var result = await Modal.Show<UpLoadForm>("", parameter, BlazoredModalOptions.GetModalOptions()).Result;
            if ((bool)result.Data)
            {
                await Upload.InvokeAsync();
            }
        }
    }
}
