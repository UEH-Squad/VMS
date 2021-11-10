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


namespace VMS.Pages.Admin.AccountManagement
{
    public partial class AccountOrgManagement : ComponentBase
    {
        private int page = 1;
        private FilterAccountViewModel filter = new();
        private List<string> selectedList = new();
        private PaginatedList<CreateAccountViewModel> pageResult = new(new(), 0, 1, 0);

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

        private async Task ShowEditAccountOrgAsync()
        {
            var result = await Modal.Show<EditAccountUser>("", BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowDeleteAccountAsync()
        {
            var result = await Modal.Show<DeleteAccount>("", BlazoredModalOptions.GetModalOptions()).Result;
        }

        private void SelectItem(string studentId)
        {
            if (IsSelectedItem(studentId))
            {
                selectedList.Remove(studentId);
            }
            else
            {
                selectedList.Add(studentId);
            }
        }

        private bool IsSelectedItem(string studentId)
        {
            return selectedList.Exists(x => x == studentId);
        }

        private void SelectAllItems()
        {
            selectedList = IsSelectedAllItems() ? new() : pageResult.Items.Select(x => x.StudentId).ToList();
        }

        private bool IsSelectedAllItems()
        {
            return pageResult.Items.Count == selectedList.Count;
        }
    }
}
