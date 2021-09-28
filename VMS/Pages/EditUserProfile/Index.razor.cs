using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.CustomValidations;

namespace VMS.Pages.EditUserProfile
{
    public partial class Index
    {

        [Inject] private ISkillService SkillService { get; set; }

        [Inject] private IJSRuntime JSRuntinme { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        private async Task<IEnumerable<SkillViewModel>> SearchSkills(string searchText)
        {
            return await SkillService.GetAllSkillsByNameAsync(searchText);
        }

        


        public class User
        {

            public string FullName { get; set; }

            public string SchoolYear { get; set; }

            public string Class { get; set; }

            public string Department { get; set; }

            public string IdStudent { get; set; }

            public string Birthday { get; set; }

            public string EmailUEH { get; set; }

            public string EmailGetNoti { get; set; }

            public string Phone { get; set; }

            public string Maxim { get; set; }

            public string Address { get; set; }

            [RequiredHasItems]
            public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
        }

        private User user = new User()
        {
            FullName = "Minh Kha Bui",
            SchoolYear = "K45",
            IdStudent = "31191025985",
            EmailUEH = "youth.bit@gmail.com",

            Class = "ST001",
            Phone = "0968790812",
            Birthday = "24/07/2001",
            EmailGetNoti = "buiminhkha24072001@gmail.com",
            Maxim = "Nợ mẹ một nàng dâu."
        };

        private IBrowserFile file;

        private int maxWord = 300;

        private int CountWord()
        {
            int count = user.Maxim.Length;
            return count;
        }

        private void FileChanged(InputFileChangeEventArgs file)
        {
            this.file = file.File;

        }

        private List<AreaViewModel> areas = new List<AreaViewModel>();
        private async Task ShowAreasModal()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
            var areasParameter = new ModalParameters();
            areasParameter.Add("choosenAreasList", areas);
            var areasModal = Modal.Show<ActivitySearchPage.AreasPopup>("", areasParameter, options);
            await areasModal.Result;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (areas.Count > 3)
            {
                await JSRuntinme.InvokeVoidAsync("vms.EditProfileCarousel");
            }
        }
        private async Task ShowSkillsPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenSkillsList", user.Skills);

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
