﻿using Blazored.Modal;
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
        private List<AddressPath> wards;
        private List<UserViewModel> organizers;
        private List<string> levels;
        private List<string> actTypes;
        private List<AreaViewModel> areasPinned;
        private bool isOrganizationShow;
        private bool isCityShow;
        private bool isDistrictShow;
        private bool isWardShow;
        private bool isLevelShow;
        private bool isActTypeShow;
        private bool isFilterDistrict;
        private bool isFilterActType;
        private bool isFilterCity;
        private bool isFilterWard;
        private bool isFilterLevel;
        private bool isFilterOrganization;
        private bool isFilterMonth;

        private bool isApproved, isNotApproved;

        private string cityChoosenValue = "Tỉnh/Thành phố";
        private string districtChoosenValue = "Quận/Huyện";
        private string wardChoosenValue = "Phường/Xã";
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
            actTypes = ActType.GetList();
        }

        private void ToggleCityDropdown()
        {
            isCityShow = !isCityShow;
        }

        private void CloseCityDropdown()
        {
            isCityShow = false;
        }

        private async Task ChooseCityValueAsync(AddressPath addressPath)
        {
            Filter.ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            isFilterCity = true;
            ToggleCityDropdown();
          
            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            isFilterDistrict = false;
            Filter.DistrictId = 0;
            districtChoosenValue = "Quận/Huyện";

            wards = new();
            isFilterWard = false;
            Filter.WardId = 0;
            wardChoosenValue = "Phường/Xã";
        }

        private void ToggleDistrictDropdown()
        {
            isDistrictShow = !isDistrictShow;
        }

        private void CloseDistrictDropdown()
        {
            isDistrictShow = false;
        }

        private async Task ChooseDistrictValueAsync(AddressPath addressPath)
        {
            Filter.DistrictId = addressPath.Id;
            districtChoosenValue = addressPath.Name;
            isFilterDistrict = true;
            ToggleDistrictDropdown();

            wards = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            isFilterWard = false;
            Filter.WardId = 0;
            wardChoosenValue = "Phường/Xã";
        }

        private void ToggleWardDropdown()
        {
            isWardShow = !isWardShow;
        }

        private void CloseWardDropdown()
        {
            isWardShow = false;
        }

        private void ChooseWardValue(AddressPath addressPath)
        {
            Filter.WardId = addressPath.Id;
            wardChoosenValue = addressPath.Name;
            isFilterWard = true;
            ToggleWardDropdown();
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

            await Modal.Show<AreasPopup>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowSkillsPopupAsync()
        {
            var skillsParameter = new ModalParameters();
            skillsParameter.Add("ChoosenSkillsList", Filter.Skills);

            await Modal.Show<SkillsPopup>("", skillsParameter, BlazoredModalOptions.GetModalOptions()).Result;
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
            Filter.ActType = actType switch
            {
                ActType.Upcoming => StatusAct.Upcoming,
                ActType.Happenning => StatusAct.Happenning,
                ActType.TookPlace => StatusAct.TookPlace,
                ActType.Closed => StatusAct.Closed,
                _ => StatusAct.All,
            };
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
            wardChoosenValue = "Phường/Xã";
            organizationChoosenValue = "Tổ chức";
            levelChoosenValue = "Cấp";
            acttypeChoosenValue = "Loại hoạt động";
            DateTimeValue = DateTime.Now;
            isFilterActType = false;
            isFilterCity = false;
            isFilterDistrict = false;
            isFilterWard = false;
            isFilterLevel = false;
            isFilterMonth = false;
            isFilterOrganization = false;
            isApproved = isNotApproved = false;
            districts = new();
            wards = new();
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

        private async Task OnChangeApprovalFilterAsync(bool isApprove)
        {
            isApproved = isApprove ? !isApproved : isApproved;
            isNotApproved = isApprove ? isNotApproved : !isNotApproved;

            Filter.IsApproved = ((isApproved && isNotApproved) || (!isApproved && !isNotApproved))
                                ? null : isApproved;

            await UpdateFilterValueAsync();
        }
    }
}
