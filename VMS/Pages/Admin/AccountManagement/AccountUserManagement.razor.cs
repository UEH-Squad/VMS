using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;
using VMS.GenericRepository;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class AccountUserManagement : ComponentBase
    {
        private int page = 1;
        private FilterAccountViewModel filter = new();
        private PaginatedList<CreateAccountViewModel> pageResult = new(new(), 0, 1, 0);

        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject] IAdminService AdminService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            filter.Role = Role.User.ToString();
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
            this.filter.Role = Role.User.ToString();
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
    }
}
