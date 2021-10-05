using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.CustomValidations;

namespace VMS.Pages.Volunteer.EditUserProfile
{
    public partial class Index
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private IUserService UserService { get; set; }

        [Inject]
        private ILogger<Index> Logger { get; set; }

        [Inject]
        private ISkillService SkillService { get; set; }

        [Inject]
        private IFacultyService FacultyService { get; set; }

        private int width;
        private string classWidth = "";
        private bool isLoading;
        private string UserId;
        private int count;
        private bool isErrorMessageShown = false;
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private List<FacultyViewModel> faculties = new();
        private CreateUserProfileViewModel user = new();

        protected override async Task OnInitializedAsync()
        {
            UserId = IdentityService.GetCurrentUserId();
            user = await UserService.GetUserProfileViewModelAsync(UserId);
            faculties = await FacultyService.GetAllFacultiesAsync();
            choosenAreas = user.Areas;
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

        private async Task ShowSkillsPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenSkillsList", user.Skills);

            await ShowModalAsync(typeof(ActivitySearchPage.SkillsPopup), parameters);
            StateHasChanged();
        }

        private async Task<IEnumerable<SkillViewModel>> SearchSkills(string searchText)
        {
            return await SkillService.GetAllSkillsByNameAsync(searchText);
        }

        private async Task ShowModalAsync(Type type, ModalParameters parameters)
        {
            ModalOptions options = new()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show(type, "", parameters, options).Result;
        }

        private int maxWord = 300;
        private int CountWord()
        {
            if (!string.IsNullOrWhiteSpace(user.Introduction))
            {
                count = user.Introduction.Length;
            }
            return count;
        }

        private async Task OnAddressChanged(int provinceId, string province, int districtId, string district, int wardId, string ward, string fullAddress)
        {
            user.ProvinceId = provinceId;
            user.Province = province;
            user.DistrictId = districtId;
            user.District = district;
            user.WardId = wardId;
            user.Ward = ward;
            user.FullAddress = fullAddress;
        }

        private async Task HandleSubmit()
        {
            if (!string.IsNullOrWhiteSpace(user.Address))
            {
                user.FullAddress = $"{user.Address}, {user.FullAddress}";
            }

            Logger.LogInformation("HandleValidSubmit called");
            isErrorMessageShown = false;
            isLoading = true;

            try
            {
                await UserService.UpdateUserProfile(user, UserId);
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

        async Task ShowConfirmPopUp()
        {
            var options = new ModalOptions()
            {

                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            Modal.Show<ConfirmNotification>("", options);
        }
    }
}
