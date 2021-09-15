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
        private ILogger<EditOrganizationProfile> Logger { get; set; }

        private bool isErrorMessageShown = false;
        private bool isLoading;
        private string OrgId;

        private CreateOrgProfileViewModel org = new();

        protected override async Task OnInitializedAsync()
        {
            OrgId = IdentityService.GetCurrentUserId();
            org = await OrgService.GetCreateOrgProfileViewModelAsync(OrgId);
        }
        private async Task HandleSubmit()
        {
            isErrorMessageShown = false;
            isLoading = true;

            try
            {
                await OrgService.UpdateOrgProfile(org, OrgId);
                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;
                Logger.LogError("Error occurs when trying to create/edit activity", ex.Message);
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        private async Task HandleInvalidSubmit()
        {
            isLoading = false;
            isErrorMessageShown = true;
            await Interop.ScrollToTop(JSRuntime);
        }

        public class Organization
        {
            [Required(ErrorMessage = "Tên tổ chức không được để trống")]
            [MinLength(2, ErrorMessage = "Tên tổ chức tối thiểu 2 kí tự")]
            [MaxLength(50, ErrorMessage = "Tên tổ chức tối đa 50 kí tự")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Email không được để trống")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Số điện thoại không được để trống")]
            [MinLength(10, ErrorMessage = "Số điện thoại tối thiểu 10 số")]
            [MaxLength(50, ErrorMessage = "Số điện thoại tối đa 12 số")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Tầm nhìn và sứ mệnh không được để trống")]
            public string Content { get; set; }
        }

        private Organization organization = new Organization()
        {
            FullName = "Công nghệ thông tin kinh doanh",
            Email = "youth.bit@gmail.com",
            Phone = "0968790812",
            Content = "This year in Tokyo, we add to a history with the Olympics that dates back to 1896 when seven members of the Harvard community brought home eight gold medals."
        };

        private IBrowserFile file;

        private int maxWord = 300;

        private int CountWord()
        {
            int count = organization.Content.Length;
            return count;
        }

        private void FileChanged(InputFileChangeEventArgs file)
        {
            this.file = file.File;

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
