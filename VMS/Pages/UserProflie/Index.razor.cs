using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Common;

namespace VMS.Pages.UserProflie
{
    public partial class Index : ComponentBase
    {
        [Parameter] public string UserId { get; set; }

        [Inject] private IIdentityService IdentityService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = IdentityService.GetCurrentUserId();

                if (string.IsNullOrEmpty(UserId))
                {
                    NavigationManager.NavigateTo(Routes.LogIn);
                }
            }

            
        }
    }
}
