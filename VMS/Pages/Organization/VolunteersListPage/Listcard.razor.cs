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
        [Parameter]
        public string SearchValue { get; set; }
        [Parameter]
        public List<int> CheckList { get; set; }
        [Parameter]
        public bool IsDeleted { get; set; }
        [Parameter]
        public PaginatedList<ListVolunteerViewModel> PagedResult { get; set; }
        [Parameter]
        public int ActId { get; set; }
        [Parameter]
        public EventCallback<List<int>> OnChangeList { get; set; }
        [Inject]
        private IListVolunteerService ListVolunteerService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        private async Task HandleCheck(int id)
        {
            var checkItem = PagedResult.Items.FirstOrDefault(x => x.Id == id);
            if (checkItem is not null)
            {
                checkItem.IsCheck = !checkItem.IsCheck;
                if (checkItem.IsCheck == true)
                {
                    CheckList.Add(id);
                }
                else
                {
                    CheckList.Remove(id);
                }
                await OnChangeList.InvokeAsync(CheckList);
            }
        }
        public async Task HandlePageChanged()
        {
            PagedResult = await ListVolunteerService.GetListVolunteers(ActId, SearchValue, !IsDeleted, page);
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
