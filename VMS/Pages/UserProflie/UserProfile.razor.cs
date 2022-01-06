using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace VMS.Pages.UserProflie
{
    public partial class UserProfile : ComponentBase
    {
        private string appellation = "Chiến sĩ hạng đồng";

        [Parameter] public bool IsUser { get; set; } = false;
        [Parameter] public UserViewModel User { get; set; } = new();
        [CascadingParameter] public IModalService Modal { get; set; }
        [CascadingParameter] public string CurrentUserId { get; set; }
        [Parameter] public bool IsUsedForAdmin { get; set; }

        [Inject] private IUploadService UploadService { get; set; }
        [Inject] private IUserService UserService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntinme.InvokeVoidAsync("vms.ProfileCarousel");
        }

        private async Task OnAvatarFileChangedAsync(InputFileChangeEventArgs e)
        {
            try
            {
                string avatar = await UploadService.SaveImageAsync(e.File, CurrentUserId, Common.Enums.ImgFolder.Avatar);

                var parameters = new ModalParameters();
                parameters.Add("Avatar", avatar);

                var options = new ModalOptions()
                {
                    HideCloseButton = true,
                    DisableBackgroundCancel = true,
                    UseCustomLayout = true,
                };

                var result = await Modal.Show<Notification>("", parameters, options).Result;

                if ((bool)result.Data)
                {
                    User.Avatar = avatar;
                    UserService.UpdateUserAvatar(CurrentUserId, avatar);
                }
                else
                {
                    UploadService.RemoveImage(avatar);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private async Task ShowModalAppellationAsync()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };

            var result = await Modal.Show<Appellation>("", options).Result;

            if (!string.IsNullOrEmpty(Convert.ToString(result.Data)))
            {
                appellation = Convert.ToString(result.Data);
            }
        }
    }
}
