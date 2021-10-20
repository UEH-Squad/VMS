﻿using System;
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
        public bool IsUseForAdmin { get; set; }

        private int width;
        private string classWidth = "";
        private string OrgId;
        private int count;
        private bool isErrorMessageShown = false;
        private IBrowserFile uploadFile;
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private CreateOrgProfileViewModel org = new();
        [CascadingParameter] public string UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OrgId = IdentityService.GetCurrentUserId();
            org = await UserService.GetOrgProfileViewModelAsync(OrgId);
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
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            var areasParameter = new ModalParameters();
            areasParameter.Add("choosenAreasList", choosenAreas);
            var areasModal = Modal.Show<ActivitySearchPage.AreasPopup>("", areasParameter, options);
            await areasModal.Result;
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

            ModalOptions options = new()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            IModalReference editConfirmModal = Modal.Show<EditConfirm>("", options);
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
                        org.Banner = await UploadService.SaveImageAsync(uploadFile, OrgId, ImgFolder.Banner);
                        UploadService.RemoveImage(oldImageName);
                    }

                    await UserService.UpdateOrgProfile(org, OrgId);

                    ModalParameters modalParams = new();
                    modalParams.Add("Title", succeededCreateTitle);
                    await Modal.Show<Activities.NotificationPopup>("", modalParams, options).Result;
                    NavigationManager.NavigateTo($"{Routes.OrgProfile}/{UserId}", true);
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
