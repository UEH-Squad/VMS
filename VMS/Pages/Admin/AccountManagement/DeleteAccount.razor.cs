using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Pages.Admin.AccountManagement
{
    public partial class DeleteAccount : ComponentBase
    {
        private bool isShowConfirm;
        [Parameter] public List<string> ListAccountIds { get; set; }
        [Parameter] public string AccountId { get; set; }

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Inject] private IAdminService AdminService { get; set; }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }

        private async Task OnAcceptDeleteAsync()
        {
            isShowConfirm = true;
            await AdminService.DeleteListAccountsAsync(ListAccountIds);
            ListAccountIds.Clear();
        }
    }
}
