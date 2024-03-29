﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;
using VMS.Domain.Models;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class SearchBoxAndFilterBar : ComponentBase
    {
        private class OrderData
        {
            public string Name { get; set; }
            public ActOrderBy OrderBy { get; set; }
        }

        private List<OrderData> orderDatas = new()
        {
            new OrderData() { Name = "Mới nhất", OrderBy = ActOrderBy.Newest },
            new OrderData() { Name = "Gần bạn nhất", OrderBy = ActOrderBy.Nearest },
            new OrderData() { Name = "Nổi bật nhất", OrderBy = ActOrderBy.Hottest }
        };

        private List<AreaViewModel> areasPinned;
        private List<AddressPath> provinces;
        private List<AddressPath> districts;
        private List<AddressPath> wards;
        private List<UserViewModel> organizers;
        private bool isOrganizationShow;
        private bool isCityShow;
        private bool isDistrictShow;
        private bool isWardShow;
        private bool isActTypeShow;
        private string cityChoosenValue = "Tỉnh/Thành phố";
        private string districtChoosenValue = "Quận/Huyện";
        private string wardChoosenValue = "Phường/Xã";
        private string organizationChoosenValue = "Tổ chức";

        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Parameter]
        public Dictionary<ActOrderBy, bool> OrderList { get; set; }
        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
        [Parameter]
        public EventCallback<Dictionary<ActOrderBy, bool>> OrderListChanged { get; set; }
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

            Filter.ActType = StatusAct.Happenning;
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
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            Filter.DistrictId = 0;
            districtChoosenValue = "Quận/Huyện";

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
            ToggleDistrictDropdown();

            wards = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
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
            ToggleOrganizationDropdown();
        }

        private void ToggleActTypeDropdown()
        {
            isActTypeShow = !isActTypeShow;
        }

        private void CloseActTypeDropdown()
        {
            isActTypeShow = false;
        }

        private void ChooseActType(StatusAct status)
        {
            Filter.ActType = status;
            ToggleActTypeDropdown();
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

            isCityShow = false;
            isDistrictShow = false;
            isWardShow = false;
            isOrganizationShow = false;
            isActTypeShow = false;

            districts = new();
            provinces = await AddressService.GetAllProvincesAsync();

            Filter = new();

            Filter.ActType = StatusAct.Happenning;

            await FilterChanged.InvokeAsync(Filter);
        }

        private async Task ChangeOrderAsync(ActOrderBy key)
        {
            OrderList[key] = !OrderList[key];
            await OrderListChanged.InvokeAsync(OrderList);
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
