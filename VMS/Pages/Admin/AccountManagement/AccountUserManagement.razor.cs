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

        [CascadingParameter] public IModalService Modal { get; set; }

        private async Task ShowEditAccountOrg()
        {
            var result = await Modal.Show<EditAccountUser>("", BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowDeleteAccount()
        {
            var result = await Modal.Show<DeleteAccount>("", BlazoredModalOptions.GetModalOptions()).Result;
        }
    }
}
