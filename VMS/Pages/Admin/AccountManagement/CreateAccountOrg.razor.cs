using VMS.Common;
using Blazored.Modal;
using VMS.Common.Enums;
using VMS.Common.Extensions;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using System.Collections.Generic;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class CreateAccountOrg : ComponentBase
    {
        private bool isLevelShow;
        private List<string> levels;
        private CreateAccountViewModel account = new() { Course = "Cấp"};
        
        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        [CascadingParameter] public IModalService ResultModal { get; set; }

        [Inject] IAdminService AdminService { get; set; }

        protected override void OnInitialized()
        {
            levels = Courses.GetLevels();
        }

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

            bool isSuccess = await AdminService.AddSingleAccountAsync(account, Role.Organization);

            await CloseModalAsync();

            ModalParameters parameters = new();
            parameters.Add("IsSuccess", isSuccess);
            ResultModal.Show<CreateFailed>("", parameters, BlazoredModalOptions.GetModalOptions());
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
