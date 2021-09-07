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

        #region Parameters

        [Parameter]
        public int ProvinceId { get; set; }

        [Parameter]
        public string Province { get; set; }

        [Parameter]
        public int DistrictId { get; set; }

        [Parameter]
        public string District { get; set; }

        [Parameter]
        public int WardId { get; set; }

        [Parameter]
        public string Ward { get; set; }

        [Parameter]
        public Func<int, string, int, string, int, string, string, Task> OnAddressChanged { get; set; }

        #endregion Parameters

        private readonly string[] defaultValues = new[]
        {
            "Tỉnh/Thành phố",
            "Quận/Huyện",
            "Phường/Xã"
        };

        private bool isCityShow;
        private bool isDistrictShow;
        private bool isWardShow;
        private List<AddressPath> provinces = new();
        private List<AddressPath> districts = new();
        private List<AddressPath> wards = new();

        protected override async Task OnInitializedAsync()
        {
            provinces = await AddressService.GetAllProvincesAsync();
            districts = await AddressService.GetAllAddressPathsByParentIdAsync(ProvinceId);
            wards = await AddressService.GetAllAddressPathsByParentIdAsync(DistrictId);
        }

        private async Task ChooseCityValue(AddressPath addressPath)
        {
            ProvinceId = addressPath.Id;
            Province = addressPath.Name;
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            DistrictId = 0;
            WardId = 0;
            District = "";
            Ward = "";

            OnAddressChanged?.Invoke(ProvinceId, Province, DistrictId, null, WardId, null, Province);
        }

        private async Task ChooseDistrictValue(AddressPath addressPath)
        {
            DistrictId = addressPath.Id;
            District = addressPath.Name;
            ToggleDistrictDropdown();

            wards = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            WardId = 0;
            Ward = "";

            OnAddressChanged?.Invoke(ProvinceId, Province, DistrictId, District, WardId, null, $"{District}, {Province}");
        }

        private void ChooseWardValue(AddressPath addressPath)
        {
            WardId = addressPath.Id;
            Ward = addressPath.Name;
            ToggleWardDropdown();

            OnAddressChanged?.Invoke(ProvinceId, Province, DistrictId, District, WardId, Ward, $"{Ward}, {District}, {Province}");
        }

        private void ToggleCityDropdown(bool isClose = false) => isCityShow = !isClose && !isCityShow;

        private void ToggleDistrictDropdown(bool isClose = false) => isDistrictShow = !isClose && !isDistrictShow;

        private void ToggleWardDropdown(bool isClose = false) => isWardShow = !isClose && !isWardShow;
    }
}