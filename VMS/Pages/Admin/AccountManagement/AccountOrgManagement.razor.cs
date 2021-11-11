using VMS.Common;
using System.Linq;
using VMS.Common.Enums;
using Microsoft.JSInterop;
using VMS.GenericRepository;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using System.Collections.Generic;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using Blazored.Modal;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class AccountOrgManagement : ComponentBase
    {
        private int page = 1;
        private FilterAccountViewModel filter = new();
        private List<string> selectedList = new();
        private PaginatedList<AccountViewModel> pageResult = new(new(), 0, 1, 0);

        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject] IAdminService AdminService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            filter.Role = Role.Organization.ToString();
            pageResult = await AdminService.GetAllAccountsAsync(filter, 1);
        }

        private async Task HandlePageChangedAsync()
        {
            pageResult = await AdminService.GetAllAccountsAsync(filter, page);
            StateHasChanged();
            await JS.InvokeVoidAsync("window.scrollTo", 0, 0);
        }

        private async Task OnFilterChangedAsync(FilterAccountViewModel filter)
        {
            this.filter = filter;
            this.filter.Role = Role.Organization.ToString();
            pageResult = await AdminService.GetAllAccountsAsync(this.filter, 1);
        }

        private void SelectItem(string accountId)
        {
            if (IsSelectedItem(accountId))
            {
                selectedList.Remove(accountId);
            }
            else
            {
                selectedList.Add(accountId);
            }
        }

        private bool IsSelectedItem(string accountId)
        {
            return selectedList.Exists(x => x == accountId);
        }

        private void SelectAllItems()
        {
            selectedList = IsSelectedAllItems() ? new() : pageResult.Items.Select(x => x.Id).ToList();
        }

        private bool IsSelectedAllItems()
        {
            return pageResult.Items.Count == selectedList.Count;
        }

        private async Task OnClickDeleteListAccountsAsync()
        {
            ModalParameters parameters = new();
            parameters.Add("ListAccountIds", selectedList);

            await Modal.Show<DeleteAccount>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            pageResult = await AdminService.GetAllAccountsAsync(filter, page);
        }

        private async Task OnClickDeleteAccountAsync(string accountId)
        {
            ModalParameters parameters = new();
            parameters.Add("ListAccountIds", new List<string>() { accountId });

            await Modal.Show<DeleteAccount>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            pageResult = await AdminService.GetAllAccountsAsync(filter, page);
        }

        private async Task ShowEditAccountAsync(AccountViewModel account)
        {
            ModalParameters parameters = new();
            parameters.Add("Account", account);

            await Modal.Show<EditAccountOrg>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }
    }
}
