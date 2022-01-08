using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.Modal;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.Extensions.Logging;
using VMS.Common;
using VMS.Common.Enums;

namespace VMS.Pages.Organization.Profile
{
    public partial class EditOrganizationProfile : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private IUserService UserService { get; set; }

        [Inject]
        private IUploadService UploadService { get; set; }

        [Inject]
        private ILogger<EditOrganizationProfile> Logger { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public bool IsUsedForAdmin { get; set; }

        [Parameter] public string UserId { get; set; }

        private string orgId;
        private int count;
        private bool isErrorMessageShown = false;
        private IBrowserFile uploadFile;
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private CreateOrgProfileViewModel org = new();

        [CascadingParameter] public string CurrentUserId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            orgId = string.IsNullOrEmpty(UserId) ? IdentityService.GetCurrentUserId() : UserId;
            org = await UserService.GetOrgProfileViewModelAsync(orgId);
            choosenAreas = org.Areas;
        }

        private int maxWord = 300;
        private int CountWord()
        {
            if (!string.IsNullOrWhiteSpace(org.Mission))
            {
                count = org.Mission.Length;
            }
            return count;
        }

        private void HandleFileChanged(InputFileChangeEventArgs args)
        {
            uploadFile = args.File;
        }

        private async Task HandleImageDiscarded()
        {
            uploadFile = null;
        }

        private async Task ShowAreasModal()
        {
            var areasParameter = new ModalParameters();
            areasParameter.Add("choosenAreasList", choosenAreas);
            await Modal.Show<ActivitySearchPage.AreasPopup>("", areasParameter, BlazoredModalOptions.GetModalOptions()).Result;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("vms.EditProfileCarousel", choosenAreas.Count);
        }

        private async Task HandleSubmit()
        {
            if (uploadFile is null && org.Banner is null)
            {
                await HandleInvalidSubmit();
                return;
            }

            IModalReference editConfirmModal = Modal.Show<EditConfirm>("", BlazoredModalOptions.GetModalOptions());
            ModalResult result = await editConfirmModal.Result;
            if (!result.Cancelled)
            {
                Logger.LogInformation("HandleValidSubmit called");
                isErrorMessageShown = false;

                try
                {
                    if (uploadFile is not null)
                    {
                        string oldImageName = org.Banner;
                        org.Banner = await UploadService.SaveImageAsync(uploadFile, orgId, ImgFolder.Banner);
                        UploadService.RemoveImage(oldImageName);
                    }

                    await UserService.UpdateOrgProfile(org, orgId);

                    ModalParameters modalParams = new();
                    modalParams.Add("Title", succeededCreateTitle);
                    await Modal.Show<Activities.NotificationPopup>("", modalParams, BlazoredModalOptions.GetModalOptions()).Result;

                    string redirectUrl = string.IsNullOrEmpty(UserId) ? $"{Routes.OrgProfile}/{org.Id}" : $"{Routes.AdminOrganizationProfile}/{org.Id}";
                    NavigationManager.NavigateTo(redirectUrl, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error occurs when trying to edit profile", ex.Message);
                    await JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        private async Task HandleInvalidSubmit()
        {
            isErrorMessageShown = true;
            await Interop.ScrollToTop(JSRuntime);
        }

    }
}
