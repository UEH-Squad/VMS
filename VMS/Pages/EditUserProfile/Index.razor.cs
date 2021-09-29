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

namespace VMS.Pages.EditUserProfile
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
            user = await UserService.GetCreateUserProfileViewModelAsync(UserId);
            faculties = await FacultyService.GetAllFacultiesAsync();
            choosenAreas = user.Areas;

            if(string.IsNullOrEmpty(user.FacultyName))
            {
                user.FacultyName = "Lựa chọn Khoa";
            }
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

        private int maxWord = 300;
        private int CountWord()
        {
            if (!string.IsNullOrWhiteSpace(user.Introduction))
            {
                count = user.Introduction.Length;
            }
            return count;
        }

        private void ChangeFacultyValue()
        {
        }

        private async Task HandleSubmit()
        {
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

        public class User
        {
            public string Address { get; set; }
        }

        private User users = new User()
        {
        };

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
