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
        private string cityChoosenValue = "Tỉnh/Thành phố";
        private string districtChoosenValue = "Quận/Huyện";
        private string organizationChoosenValue = "Tổ chức";

        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Parameter]
        public bool[] OrderList { get; set; }
        [Parameter]
        public string SearchValue { get; set; }
        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
        [Parameter]
        public EventCallback<bool[]> OrderListChanged { get; set; }
        [Parameter]
        public EventCallback<string> SearchValueChanged { get; set; }
        [Parameter]
        public EventCallback<FilterActivityViewModel> FilterChanged { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set;  }
        [Inject]
        private IAddressService AddressService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Filter = new FilterActivityViewModel();

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
            Filter.ProvinceId = addressPath.Id;
            cityChoosenValue = addressPath.Name;
            ToggleCityDropdown();

            districts = await AddressService.GetAllAddressPathsByParentIdAsync(addressPath.Id);
            Filter.DistrictId = 0;
            districtChoosenValue = "Quận/Huyện";
        }

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

        private void ToggleOrganizationDropdown()
        {
            isOrganizationShow = !isOrganizationShow;
        }
        private void ChooseOrganizationValue(User organizer)
        {
            Filter.OrgId = organizer.UserName;
            organizationChoosenValue = organizer.UserName;
            ToggleOrganizationDropdown();
        }

        private async Task UpdateSearchValueAsync(ChangeEventArgs e)
        {
            SearchValue = (string)e.Value;
            await SearchAsync();
        }
        private async Task SearchAsync()
        {
            await SearchValueChanged.InvokeAsync(SearchValue);
        }
        private void ClearSearchBox()
        {
            SearchValue = string.Empty;
            StateHasChanged();
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

        private async Task UpdateFilterValueAsync()
        {
            await FilterChanged.InvokeAsync(Filter);
        }

        private void ClearFilter()
        {
            cityChoosenValue = "Tỉnh/Thành phố";
            districtChoosenValue = "Quận/Huyện";
            organizationChoosenValue = "Tổ chức";
            Filter = new FilterActivityViewModel();
        }

        private async Task ChangeOrderAsync(int id)
        {
            OrderList[id] = !OrderList[id];
            await OrderListChanged.InvokeAsync(OrderList);
        }
    }
}
