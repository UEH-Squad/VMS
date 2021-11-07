using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class Listcard : ComponentBase
    {
        private int page = 1;
        private string searchValue = String.Empty;
        private List<int> checkList = new();
        private bool isDeleted = false;
        private PaginatedList<ListVolunteerViewModel> pagedResult = new(new(), 0, 1, 1);
        [Parameter]
        public int ActId { get; set; }
        [Inject]
        private IRecruitmentService ListVolunteerService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            pagedResult = await ListVolunteerService.GetListVolunteersAsync(ActId, searchValue, isDeleted, page);
        }
        private async Task ValueChangeAsync(string value)
        {
            page = 1;
            this.searchValue = value;
            pagedResult = await ListVolunteerService.GetListVolunteersAsync(ActId, searchValue, isDeleted, page);
            checkList = new();
            StateHasChanged();
        }
        private async Task ShowDeletedListAsync(bool value)
        {
            page = 1;
            this.isDeleted = value;
            searchValue = String.Empty;
            pagedResult = await ListVolunteerService.GetListVolunteersAsync(ActId, searchValue, isDeleted, page);
            checkList = new();
            StateHasChanged();
        }
        private async Task HandleCheckAsync(int id)
        {
            var checkItem = pagedResult.Items.FirstOrDefault(x => x.Id == id);
            if (checkItem is not null)
            {
                checkItem.IsCheck = !checkItem.IsCheck;
                if (checkItem.IsCheck == true)
                {
                    checkList.Add(id);
                }
                else
                {
                    checkList.Remove(id);
                }
            }
        }
        public async Task HandlePageChangedAsync()
        {
            this.checkList = new();
            pagedResult = await ListVolunteerService.GetListVolunteersAsync(ActId, searchValue, isDeleted, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }
        public async Task HandleDeletedAsync()
        {
            if (pagedResult.TotalItems - checkList.Count <= (pagedResult.PageIndex - 1) * pagedResult.PageSize)
            {
                page = pagedResult.PageIndex - 1;
            }
            pagedResult = await ListVolunteerService.GetListVolunteersAsync(ActId, searchValue, isDeleted, page);
            checkList = new();
            StateHasChanged();
        }
        [CascadingParameter] public IModalService Modal { get; set; }
        async Task ShowSignUpPopUpAsync(ListVolunteerViewModel volunteer)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            ModalParameters parameters = new();
            parameters.Add("ActivityId", 1);
            parameters.Add("Volunteer", volunteer);
            parameters.Add("CurrentUser", volunteer.User);
            parameters.Add("IsReadOnly", true);
            Modal.Show<VMS.Pages.ActivitySearchPage.Signup>("", parameters, options);
        }
    }
}
