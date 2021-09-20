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
        private bool isErrorMessageShown = false;
        private IBrowserFile uploadFile;
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private bool isEditConfirm = false;
        private bool isEditSuccess = false;

        private CreateOrgProfileViewModel org = new();

        protected override async Task OnInitializedAsync()
        {
            OrgId = IdentityService.GetCurrentUserId();
            org = await OrgService.GetCreateOrgProfileViewModelAsync(OrgId);
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
            isPreview = true;
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
            if (choosenAreas.Count > 3)
            {
                await JSRuntime.InvokeVoidAsync("vms.EditProfileCarousel");
            }
        }

        void EditConfirm()
        {
            isEditConfirm = !isEditConfirm;
        }

        void EditSuccess()
        {
            isEditSuccess = !isEditSuccess;
            if (isEditConfirm == true)
            {
                isEditConfirm = false;
            }
        }

        private async Task HandleSubmit()
        {
            Logger.LogInformation("HandleValidSubmit called");
            isErrorMessageShown = false;
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
            isErrorMessageShown = true;
            await Interop.ScrollToTop(JSRuntime);
        }

    }
}
