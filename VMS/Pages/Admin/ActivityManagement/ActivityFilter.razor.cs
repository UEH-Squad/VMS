using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;
using VMS.Domain.Models;
using VMS.Pages.ActivitySearchPage;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class ActivityFilter : ComponentBase
    {
        private List<AddressPath> provinces;
        private List<AddressPath> districts;
        private List<UserViewModel> organizers;
        private List<string> levels;
        private List<string> actTypes;
        private List<AreaViewModel> areasPinned;
        private bool isOrganizationShow;
        private bool isCityShow;
        private bool isDistrictShow;
        private bool isLevelShow;
        private bool isActTypeShow;
        private bool isFilterDistrict;
        private bool isFilterActType;
        private bool isFilterCity;
        private bool isFilterLevel;
        private bool isFilterOrganization;
        private bool isFilterMonth;
        private string cityChoosenValue = "Tỉnh/Thành phố";
        private string districtChoosenValue = "Quận/Huyện";
        private string organizationChoosenValue = "Tổ chức";
        private string levelChoosenValue = "Cấp";
        private string acttypeChoosenValue = "Loại hoạt động";
        [Parameter] public DateTime DateTimeValue { get; set; }
        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
        [Parameter]
        public EventCallback<FilterActivityViewModel> FilterChanged { get; set; }
        [Inject]
        private IOrganizationService OrganizationService { get; set; }
        [Inject]
        private IAddressService AddressService { get; set; }
        [Inject]
        private IAreaService AreaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            organizers = OrganizationService.GetAllOrganizers();
            isOrganizationShow = false;

            provinces = await AddressService.GetAllProvincesAsync();
            areasPinned = await AreaService.GetAllAreasAsync(true);

            levels = Courses.GetLevels();
            actTypes = ActType.GetType();
        }

        private void ToggleCityDropdown()
        {
            isCityShow = !isCityShow;
        }

        private void CloseCityDropdown()
        {
            isCityShow = false;
        }

        private async Task ChooseCityValue(AddressPath addressPath)
        {
            Filter.ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            isFilterCity = true;
            ToggleCityDropdown();
          
            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            Filter.DistrictId = 0;
            districtChoosenValue = "Quận/Huyện";
        }

        private void ToggleDistrictDropdown()
        {
            isDistrictShow = !isDistrictShow;
        }

        private void CloseDistrictDropdown()
        {
            isDistrictShow = false;
        }

        private void ChooseDistrictValue(AddressPath addressPath)
        {
            Filter.DistrictId = addressPath.Id;
            districtChoosenValue = addressPath.Name;
            isFilterDistrict = true;
            ToggleDistrictDropdown();
        }

        private void ToggleOrganizationDropdown()
        {
            isOrganizationShow = !isOrganizationShow;
        }

        private void CloseOrganizationDropdown()
        {
            isOrganizationShow = false;
        }

        private void ChooseOrganizationValue(UserViewModel organizer)
        {
            Filter.OrgId = organizer.Id;
            organizationChoosenValue = organizer.FullName;
            isFilterOrganization = true;
            ToggleOrganizationDropdown();
        }

        private async Task ShowAreasPopupAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", Filter.Areas);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<AreasPopup>("", parameters, options).Result;
        }

        private async Task ShowSkillsPopupAsync()
        {
            var skillsParameter = new ModalParameters();
            skillsParameter.Add("ChoosenSkillsList", Filter.Skills);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<SkillsPopup>("", skillsParameter, options).Result;
        }
        private void ToggleLevelDropdown()
        {
            isLevelShow = !isLevelShow;
        }
        private void ChooseLevelValue(string level)
        {
            levelChoosenValue = level;
            Filter.Level = level;
            isFilterLevel = true;
        }
        private void CloseLevelDropdown()
        {
            isLevelShow = false;
        }
        private void ToggleActTypeDropdown()
        {
            isActTypeShow = !isActTypeShow;
        }
        private void ChooseActTypeValue(string actType)
        {
            switch (actType){
                case ActType.upComing: Filter.ActType = StatusAct.Upcoming; break;
                case ActType.happenning: Filter.ActType = StatusAct.Happenning; break;
                case ActType.tookPlace: Filter.ActType = StatusAct.TookPlace; break;
                case ActType.closed: Filter.ActType = StatusAct.Closed; break;
            } 
            acttypeChoosenValue = actType;
            isFilterActType = true;
        }
        private void CloseActTypeDropdown()
        {
            isActTypeShow = false;
        }
        void ChooseMonthValue()
        {
            Filter.IsMonthFilter = true;
            Filter.DateTimeValue = DateTimeValue;
            isFilterMonth = true;
        }
        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(Filter);
        }

        private async Task ClearFilterAsync()
        {
            cityChoosenValue = "Tỉnh/Thành phố";
            districtChoosenValue = "Quận/Huyện";
            organizationChoosenValue = "Tổ chức";
            levelChoosenValue = "Cấp";
            acttypeChoosenValue = "Loại hoạt động";
            DateTimeValue = DateTime.Now;
            isFilterActType = false;
            isFilterCity = false;
            isFilterDistrict = false;
            isFilterLevel = false;
            isFilterMonth = false;
            isFilterOrganization = false;
            districts = new();
            provinces = await AddressService.GetAllProvincesAsync();
            Filter = new();
            await FilterChanged.InvokeAsync(Filter);
        }
        private void ChangeStatePinnedArea(AreaViewModel areaViewModel)
        {
            AreaViewModel area = Filter.Areas.Find(a => a.Id == areaViewModel.Id);
            if (area is null)
            {
                Filter.Areas.Add(areaViewModel);
            }
            else
            {
                Filter.Areas.Remove(area);
            }
        }
    }
}
