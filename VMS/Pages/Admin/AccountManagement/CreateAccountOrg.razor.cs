using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Common.Extensions;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountOrg : ComponentBase
    {
        private bool? isSuccess;
        private bool isLevelShow;
        private CreateAccountViewModel account = new() { Course = "Cấp"};
        private List<string> levels = new()
        {
            "Ban Chuyên môn",
            "Khoa/Viện/KTX",
            "CLB/Đội/Nhóm"
        };
        
        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] IAdminService AdminService { get; set; }

        private void ChooseLevelValue(string level)
        {
            account.Course = level;
        }
        private void ToggLeLevelDropdown()
        {
            isLevelShow = !isLevelShow;
        }

        private void CloseLevelDropdown()
        {
            isLevelShow = false;
        }

        private async Task OnValidSubmitAsync()
        {
            if (!IsValidAccount())
            {
                return;
            }

            isSuccess = await AdminService.AddSingleAccountAsync(account, Role.Organization);
        }

        private bool IsValidAccount()
        {
            return levels.Exists(x => x == account.Course) && account.IsValidAccount(Role.Organization);
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
