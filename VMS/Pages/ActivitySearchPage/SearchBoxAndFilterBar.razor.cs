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
        private FilterActivityViewModel filter;
        private bool isOrganizationShow;
        private bool isCityShow;
        private bool isDistrictShow;
        private string cityChoosenValue = "Tỉnh/Thành phố";
        private string districtChoosenValue = "Quận/Huyện";
        private string organizationChoosenValue = "Tổ chức";
        private string searchValue;


        [Parameter]
        public EventCallback<string> SearchValueChanged { get; set; }
        [Parameter]
        public EventCallback<FilterActivityViewModel> FilterValueChanged { get; set; }
        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set;  }
        [Inject]
        private IAddressService AddressService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            filter = new FilterActivityViewModel();

            organizers = IdentityService.GetAllOrganizers();
            isOrganizationShow = false;

            provinces = await AddressService.GetAllProvincesAsync();
        }

        private void ToggleCityDropdown()
        {
            isCityShow = !isCityShow;
        }
        private async Task ChooseCityValue(AddressPath addressPath)
        {
            filter.ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            filter.DistrictId = 0;
            districtChoosenValue = "Quận/Huyện";
        }

        private void ToggleDistrictDropdown()
        {
            isDistrictShow = !isDistrictShow;
        }
        private void ChooseDistrictValue(AddressPath addressPath)
        {
            filter.DistrictId = addressPath.Id;
            districtChoosenValue = addressPath.Name;
            ToggleDistrictDropdown();
        }

        private void ToggleOrganizationDropdown()
        {
            isOrganizationShow = !isOrganizationShow;
        }
        private void ChooseOrganizationValue(User organizer)
        {
            filter.OrgId = organizer.UserName;
            organizationChoosenValue = organizer.UserName;
            ToggleOrganizationDropdown();
        }

        private async Task UpdateSearchValue(ChangeEventArgs e)
        {
            searchValue = (string)e.Value;
            await SearchAsync();
        }
        private async Task SearchAsync()
        {
            await SearchValueChanged.InvokeAsync(searchValue);
        }
        private void ClearSearchBox()
        {
            searchValue = string.Empty;
            StateHasChanged();
        }

        private async Task ShowAreasPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenAreasList", filter.Areas);

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
            skillsParameter.Add("ChoosenSkillsList", filter.Skills);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            await Modal.Show<SkillsPopup>("", skillsParameter, options).Result;
        }

        private async Task UpdateFilterValue()
        {
            await FilterValueChanged.InvokeAsync(filter);
        }

        void ClearFilter()
        {
            cityChoosenValue = "Tỉnh/Thành phố";
            districtChoosenValue = "Quận/Huyện";
            organizationChoosenValue = "Tổ chức";
            filter = new FilterActivityViewModel();
        }
    }
}
