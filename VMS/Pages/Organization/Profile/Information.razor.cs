using System.Threading.Tasks;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using VMS.Application.Services;
using Blazored.Modal;

namespace VMS.Pages.Organization.Profile
{
    public partial class Information : ComponentBase
    {
        private string message;
        private string avatar;
        private IBrowserFile file;
        private OrgRatingViewModel org;
        [Parameter]
        public bool Owner { get; set; }
        [Parameter]
        public string UserId { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
         [Inject]
        protected IUploadService UploadService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        protected override void OnInitialized()
        {
            org = OrganizationService.GetOrgRating(UserId);
            if (!string.Equals(UserId, IdentityService.GetCurrentUserId()))
            {
                Owner = false;
            }
            else Owner = true;
        }

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };
            Modal.Show<Notification>("", options);
            if (e.File.ContentType != "image/jpeg")
            {
                message = $"File không đúng định dạng..";
                this.StateHasChanged();
            }
            else
            {
                message = "";
                file = e.File;
                avatar = await UploadService.GetDataUriAsync(file);
            }
        }
    }

}