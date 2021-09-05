using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Pages.Organization.Activities
{
    public partial class CascadingAddressPicker : ComponentBase
    {
        [Inject]
        private IAddressService AddressService { get; set; }

        [Parameter]
        public int ProvinceId { get; set; }

        [Parameter]
        public int DistrictId { get; set; }

        [Parameter]
        public int WardId { get; set; }

        [Parameter]
        public Action<int, int, int> OnAddressChanged { get; set; }

        private string[] defaultValues;
        private bool isCityShow;
        private bool isDistrictShow;
        private bool isWardShow;
        private string cityChoosenValue;
        private string districtChoosenValue;
        private string wardChoosenValue;
        private List<AddressPath> provinces = new();
        private List<AddressPath> districts = new();
        private List<AddressPath> wards = new();

        protected override async Task OnInitializedAsync()
        {
            provinces = await AddressService.GetAllProvincesAsync();
            defaultValues = new[]
            {
                "Tỉnh/Thành phố",
                "Quận/Huyện",
                "Phường/Xã"
            };
            cityChoosenValue = defaultValues[0];
            districtChoosenValue = defaultValues[1];
            wardChoosenValue = defaultValues[2];
        }

        private async Task ChooseCityValue(AddressPath addressPath)
        {
            ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            DistrictId = 0;
            WardId = 0;
            districtChoosenValue = defaultValues[1];
            wardChoosenValue = defaultValues[2];

            OnAddressChanged?.Invoke(ProvinceId, DistrictId, WardId);
        }

        private async Task ChooseDistrictValue(AddressPath addressPath)
        {
            DistrictId = addressPath.Id;
            districtChoosenValue = addressPath.Name;
            ToggleDistrictDropdown();

            wards = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            WardId = 0;
            wardChoosenValue = defaultValues[2];

            OnAddressChanged?.Invoke(ProvinceId, DistrictId, WardId);
        }

        private void ChooseWardValue(AddressPath addressPath)
        {
            WardId = addressPath.Id;
            wardChoosenValue = addressPath.Name;
            ToggleWardDropdown();

            OnAddressChanged?.Invoke(ProvinceId, DistrictId, WardId);
        }

        private void ToggleCityDropdown(bool isClose = false) => isCityShow = !isClose && !isCityShow;

        private void ToggleDistrictDropdown(bool isClose = false) => isDistrictShow = !isClose && !isDistrictShow;

        private void ToggleWardDropdown(bool isClose = false) => isWardShow = !isClose && !isWardShow;
    }
}