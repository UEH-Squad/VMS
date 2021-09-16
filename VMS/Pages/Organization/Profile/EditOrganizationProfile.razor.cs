using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.Modal;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.Extensions.Logging;
using VMS.Pages.Organization.Activities;
using VMS.Common;
using VMS.Domain.Models;

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
        private IOrgService OrgService { get; set; }

        [Inject]
        private IUploadService UploadService { get; set; }

        [Inject]
        private ILogger<EditOrganizationProfile> Logger { get; set; }

        private bool isLoading;
        private bool isPreview;
        private string OrgId;
        private int count;
        private IBrowserFile uploadFile;

        private CreateOrgProfileViewModel org = new();

        protected override async Task OnInitializedAsync()
        {
            OrgId = IdentityService.GetCurrentUserId();
            org = await OrgService.GetCreateOrgProfileViewModelAsync(OrgId);
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
            isPreview = true;
        }

        private async Task HandleImageDiscarded()
        {
            uploadFile = null;
        }

        private async Task HandleSubmit()
        {
            Logger.LogInformation("HandleValidSubmit called");
            isLoading = true;
            isPreview = false;

            try
            {
                if (uploadFile is not null)
                {
                    string oldImageName = org.Avatar;
                    org.Avatar = await UploadService.SaveImageAsync(uploadFile, OrgId);
                    UploadService.RemoveImage(oldImageName);
                }
                await OrgService.UpdateOrgProfile(org, OrgId);
                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;
                Logger.LogError("Error occurs when trying to edit profile", ex.Message);
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        private async Task HandleInvalidSubmit()
        {
            isLoading = false;
            await Interop.ScrollToTop(JSRuntime);
        }

        bool[] choosenAreasList = new bool[12];
        private async Task ShowAreasModal()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            var areasParameter = new ModalParameters();
            areasParameter.Add("choosenAreasList", choosenAreasList);
            var areasModal = Modal.Show<ActivitySearchPage.AreasPopup>("", areasParameter, options);
            await areasModal.Result;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("vms.EditProfileCarousel");
        }
    }
}
