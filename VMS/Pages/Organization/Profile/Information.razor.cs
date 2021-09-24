using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.Modal.Services;
using System.Threading.Tasks;
using Blazored.Modal;

namespace VMS.Pages.Organization.Profile
{
    public partial class Information : ComponentBase
    {
        private UserViewModel org;
        [Parameter]
        public string UserId { get; set; }
        [Parameter]
        public bool Owner { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        protected override void OnInitialized()
        {
            org = OrganizationService.GetOrgFull(UserId);
        }
        public string avatar;
        public IBrowserFile file;
        [Inject]
        protected IUploadService UploadService { get; set; }
        [CascadingParameter]
        public IModalService Modal { get; set; }

        async Task ShowModal(InputFileChangeEventArgs e)
        {
            if (e.File.ContentType == "image/jpeg")
            {
                file = e.File;
                avatar = await UploadService.GetDataUriAsync(file);
            }
            var parameters = new ModalParameters();
            parameters.Add("avatar", avatar);
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };
           var result = await Modal.Show<Notification>("", parameters, options).Result;
            if ((bool)result.Data)
            {
                org.Avatar = avatar;
            }

        }

        private bool HaftStar(double rate, int star)
        {
            if (rate - star > 0 && rate - star <= 0.5)
            {
                return true;
            }
            return false;
        }
    }

}