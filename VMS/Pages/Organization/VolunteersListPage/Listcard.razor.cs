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
        private IListVolunteerService ListVolunteerService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            pagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, isDeleted, page);
        }
        private async Task ValueChange(string value)
        {
            this.searchValue = value;
            pagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, isDeleted, page);
            StateHasChanged();
        }
        private async Task ShowDeletedList(bool value)
        {
            this.isDeleted = value;
            pagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, isDeleted, page);
            StateHasChanged();
        }
        private async Task HandleCheck(int id)
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
        public async Task HandlePageChanged()
        {
            this.checkList = new();
            pagedResult = await ListVolunteerService.GetListVolunteers(ActId, searchValue, isDeleted, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        [CascadingParameter] public IModalService Modal { get; set; }
        async Task ShowSignUpPopUp(ListVolunteerViewModel volunteer)
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
