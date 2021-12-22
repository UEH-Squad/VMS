using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Pages.Organization.Profile;

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

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private string UserId;
        private int count;
        private bool isErrorMessageShown = false;
        private string facultyChoosenValue = "Lựa chọn Khoa";
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private List<FacultyViewModel> faculties = new();
        private CreateUserProfileViewModel user = new();

        protected override async Task OnInitializedAsync()
        {
            UserId = IdentityService.GetCurrentUserId();
            user = await UserService.GetUserProfileViewModelAsync(UserId);
            faculties = await FacultyService.GetAllFacultiesAsync();
            choosenAreas = user.Areas;
            if(user.FacultyId is not null)
            {
                facultyChoosenValue = user.FacultyName;
            }
        }

        private async Task ShowAreasModal()
        {
            var areasParameter = new ModalParameters();
            areasParameter.Add("choosenAreasList", choosenAreas);
            await Modal.Show<ActivitySearchPage.AreasPopup>("", areasParameter, BlazoredModalOptions.GetModalOptions()).Result;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (choosenAreas.Count > 3)
            {
                await JSRuntime.InvokeVoidAsync("vms.EditProfileCarousel");
            }
        }

        private void ChooseDepartmentValue(FacultyViewModel faculty)
        {
            facultyChoosenValue = faculty.Name;
            user.FacultyId = faculty.Id.ToString();
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
            await Modal.Show(type, "", parameters, BlazoredModalOptions.GetModalOptions()).Result;
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
            IModalReference editConfirmModal = Modal.Show<EditConfirm>("", BlazoredModalOptions.GetModalOptions());
            ModalResult result = await editConfirmModal.Result;
            if (!result.Cancelled)
            {
                if (!string.IsNullOrWhiteSpace(user.Address))
                {
                    user.FullAddress = $"{user.Address}, {user.FullAddress}";
                }

                Logger.LogInformation("HandleValidSubmit called");
                isErrorMessageShown = false;

                try
                {
                    await UserService.UpdateUserProfile(user, UserId);

                    ModalParameters modalParams = new();
                    modalParams.Add("Title", succeededCreateTitle);
                    await Modal.Show<Organization.Activities.NotificationPopup>("", modalParams, BlazoredModalOptions.GetModalOptions()).Result;
                    NavigationManager.NavigateTo($"{Routes.UserProfile}/{UserId}", true);
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
