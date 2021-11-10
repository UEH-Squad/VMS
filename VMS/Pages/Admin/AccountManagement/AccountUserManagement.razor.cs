using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
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
    }
}
