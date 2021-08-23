using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class SearchBoxAndFilterBar : ComponentBase
    {
        private List<AddressPath> provinces;
        private List<AddressPath> districts;
        private List<User> organizers;
        private bool isOrganizationShow;
        private bool isCityShow;
        private bool isDistrictShow;

        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set;  }
        [Inject]
        private IAddressService AddressService { get; set; }

        string cityChoosenValue = "Tỉnh/Thành phố";
        string districtChoosenValue = "Quận/Huyện";
        string organizationChoosenValue = "Tổ chức";

        string mySearchValue = "";

        protected override async Task OnInitializedAsync()
        {
            Filter = new FilterActivityViewModel();

            organizers = IdentityService.GetAllOrganizers();
            isOrganizationShow = false;

            provinces = await AddressService.GetAllProvincesAsync();
        }

        //function for city dropdowns
        private void ToggleCityDropdown()
        {
            isCityShow = !isCityShow;
        }
        private async Task ChooseCityValue(AddressPath addressPath)
        {
            Filter.ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            Filter.DistrictId = 0;
            districtChoosenValue = "Quận/Huyện";
        }

        //function for district dropdowns
        private void ToggleDistrictDropdown()
        {
            isDistrictShow = !isDistrictShow;
        }
        private void ChooseDistrictValue(AddressPath addressPath)
        {
            Filter.DistrictId = addressPath.Id;
            districtChoosenValue = addressPath.Name;
            ToggleDistrictDropdown();
        }

        //function for organization dropdowns
        void ToggleOrganizationDropdown()
        {
            isOrganizationShow = !isOrganizationShow;
        }
        void ChooseOrganizationValue(User organizer)
        {
            Filter.OrgId = organizer.UserName;
            organizationChoosenValue = organizer.UserName;
            ToggleOrganizationDropdown();
        }

        //Clear search box button
        void UpdateInstanceValue(ChangeEventArgs e)
        {
            mySearchValue = e.Value.ToString();
        }
        void ClearSearchBox()
        {
            mySearchValue = string.Empty;
        }

        private async Task ShowAreasPopup()
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


        private async Task ShowSkillsPopup()
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

        //clear filter bar
        void ClearFilter()
        {
            cityChoosenValue = "Tỉnh/Thành phố";
            districtChoosenValue = "Quận/Huyện";
            organizationChoosenValue = "Tổ chức";
            Filter = new FilterActivityViewModel();
        }
    }
}
