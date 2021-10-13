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
        [Parameter] 
        public string UserId { get; set; }
        [Parameter] public UserViewModel Org { get; set; }
        [Parameter] public bool HaveControl { get; set; }
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
                avatar = await UploadService.SaveImageAsync(file, UserId);//this code will save image to ./img/activities, need to improve later
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
                Org.Avatar = avatar;
            }

        }

        private static bool HaftStar(double rate, int star)
        {
            if (rate - star > 0 && rate - star <= 0.5)
            {
                return true;
            }
            return false;
        }
    }

}