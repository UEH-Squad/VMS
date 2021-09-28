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
        private ISkillService SkillService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private IUserService UserService { get; set; }

        [Inject]
        private ILogger<Index> Logger { get; set; }

        private bool isLoading;
        private string UserId;
        private bool isErrorMessageShown = false;
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private CreateUserProfileViewModel user = new();

        protected override async Task OnInitializedAsync()
        {
            UserId = IdentityService.GetCurrentUserId();
            user = await UserService.GetCreateUserProfileViewModelAsync(UserId);
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

        private async Task<IEnumerable<SkillViewModel>> SearchSkills(string searchText)
        {
            return await SkillService.GetAllSkillsByNameAsync(searchText);
        }

        public class User
        {
            public string SchoolYear { get; set; }

            public string Class { get; set; }

            public string Department { get; set; }

            public string Birthday { get; set; }

            public string EmailUEH { get; set; }

            public string EmailGetNoti { get; set; }

            public string Maxim { get; set; }

            public string Address { get; set; }

            [RequiredHasItems]
            public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
        }

        private User users = new User()
        {
            SchoolYear = "K45",

            Class = "ST001",
            Birthday = "24/07/2001",
            Maxim = "Nợ mẹ một nàng dâu."
        };

        private IBrowserFile file;

        private int maxWord = 300;

        private int CountWord()
        {
            int count = users.Maxim.Length;
            return count;
        }

        private void FileChanged(InputFileChangeEventArgs file)
        {
            this.file = file.File;

        }

        private async Task ShowSkillsPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenSkillsList", users.Skills);

            await ShowModalAsync(typeof(ActivitySearchPage.SkillsPopup), parameters);
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

        private string departmentChoosenValue = "Lựa chọn Khoa";

        private string nameDepartmentValue = "Khoa Công nghệ thông tin Kinh doanh";

        void ChooseDepartmentValue(int id)
        {
            departmentChoosenValue = nameDepartmentValue + " " + (id).ToString();
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
