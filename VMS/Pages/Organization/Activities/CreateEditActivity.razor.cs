using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.Organization.Activities
{
    public partial class CreateEditActivity : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject]
        private IAddressService AddressService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private ISkillService SkillService { get; set; }

        private readonly Fake activity = new()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7)
        };

        private bool isCityShow;
        private bool isDistrictShow;
        private bool isWardShow;
        private string cityChoosenValue = "Tỉnh/Thành phố";
        private string districtChoosenValue = "Quận/Huyện";
        private string wardChoosenValue = "Phường/Xã";
        private List<AddressPath> provinces;
        private List<AddressPath> districts;
        private List<AddressPath> wards;

        private readonly List<string> targets = new()
        {
            "Sinh viên năm nhất",
            "Sinh viên năm hai",
            "Sinh viên năm ba",
            "Sinh viên năm tư"
        };

        protected override async Task OnInitializedAsync()
        {
            provinces = await AddressService.GetAllProvincesAsync();
        }

        private async Task ChooseCityValue(AddressPath addressPath)
        {
            activity.ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            activity.DistrictId = 0;
            activity.WardId = 0;
            districtChoosenValue = "Quận/Huyện";
            wardChoosenValue = "Phường/Xã";
        }

        private async Task ChooseDistrictValue(AddressPath addressPath)
        {
            activity.DistrictId = addressPath.Id;
            districtChoosenValue = addressPath.Name;
            ToggleDistrictDropdown();

            wards = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            activity.WardId = 0;
            wardChoosenValue = "Phường/Xã";
        }

        private void ChooseWardValue(AddressPath addressPath)
        {
            activity.WardId = addressPath.Id;
            wardChoosenValue = addressPath.Name;
            ToggleWardDropdown();
        }

        private void ToggleCityDropdown()
        {
            isCityShow = !isCityShow;
        }

        private void ToggleDistrictDropdown()
        {
            isDistrictShow = !isDistrictShow;
        }

        private void ToggleWardDropdown()
        {
            isWardShow = !isWardShow;
        }

        private void CloseCityDropdown()
        {
            isCityShow = false;
        }

        private void CloseDistrictDropdown()
        {
            isDistrictShow = false;
        }

        private void CloseWardDropdown()
        {
            isWardShow = false;
        }

        private async Task OnStartDateChanged(ChangeEventArgs args)
        {
            DateTime selectedDate = DateTime.Parse(args.Value.ToString());
            if (selectedDate <= DateTime.Now)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Ngày bắt đầu phải sau ngày hôm nay!");
                selectedDate = DateTime.Now;
            }

            activity.StartDate = selectedDate;
        }
        private async Task OnEndDateChanged(ChangeEventArgs args)
        {
            DateTime selectedDate = DateTime.Parse(args.Value.ToString());
            if (selectedDate < DateTime.Now)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Ngày kết thúc phải sau ngày bắt đầu!");
                selectedDate = DateTime.Now.AddDays(7);
            }

            activity.EndDate = selectedDate;
        }

        private async Task ShowAreasPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", activity.Areas);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<ActivitySearchPage.AreasPopup>("", parameters, options).Result;
        }

        private async Task ShowSkillsPopup()
        {
            var skillsParameter = new ModalParameters();
            skillsParameter.Add("ChoosenSkillsList", activity.Skills);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<ActivitySearchPage.SkillsPopup>("", skillsParameter, options).Result;
        }

        private async Task<IEnumerable<string>> SearchTargets(string searchText)
        {
            return await Task.FromResult(targets.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList());
        }

        private async Task<IEnumerable<SkillViewModel>> SearchSkills(string searchText)
        {
            return await SkillService.GetAllSkillsByNameAsync(searchText);
        }

        private class Fake : Activity
        {
            public int ProvinceId { get; set; }
            public int DistrictId { get; set; }
            public int WardId { get; set; }
            public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
            public IList<int> Areas { get; set; } = new List<int>();
            public IList<string> Targets { get; set; }
        }
    }
}