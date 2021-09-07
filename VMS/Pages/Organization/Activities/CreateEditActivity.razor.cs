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
using VMS.Common;

namespace VMS.Pages.Organization.Activities
{
    public partial class CreateEditActivity : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ISkillService SkillService { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }

        private IList<string> chosenTargets = new List<string>();
        private IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private bool isErrorMessageShown = false;

        private readonly CreateActivityViewModel activity = new()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7)
        };

        private readonly List<string> targets = new()
        {
            "Năm nhất",
            "Năm hai",
            "Năm ba",
            "Năm tư",
            "Tất cả mọi đối tượng"
        };

        private async Task OnAddressChanged(int provinceId, int districtId, int wardId, string address)
        {
            activity.ProvinceId = provinceId;
            activity.DistrictId = districtId;
            activity.WardId = wardId;
            activity.FullAddress = address;
        }

        private async Task HandleFileChanged(IBrowserFile file)
        {
            activity.Banner = file.Name;
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
            ModalParameters parameters = new();
            parameters.Add("ChoosenAreasList", choosenAreas);
            parameters.Add("IsSingleArea", true);

            await ShowModalAsync(typeof(ActivitySearchPage.AreasPopup), parameters);
            activity.AreaId = choosenAreas.FirstOrDefault()?.Id ?? 0;
        }

        private async Task ShowSkillsPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add("ChoosenSkillsList", activity.Skills);

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

        private async Task<IEnumerable<string>> SearchTargets(string searchText)
        {
            return await Task.FromResult(targets.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList());
        }

        private async Task<IEnumerable<SkillViewModel>> SearchSkills(string searchText)
        {
            return await SkillService.GetAllSkillsByNameAsync(searchText);
        }

        private async Task HandleSubmit()
        {
            if (!string.IsNullOrWhiteSpace(activity.Address))
            {
                activity.FullAddress = $"{activity.Address}, {activity.FullAddress}";
            }

            activity.Targets = string.Join('|', chosenTargets);
            if (string.IsNullOrWhiteSpace(activity.Targets))
            {
                await HandleInvalidSubmit();
                return;
            }

            isErrorMessageShown = false;
            //await ActivityService.AddActivityAsync(activity);
            await JSRuntime.InvokeVoidAsync("console.log", activity);
            await ShowModalAsync(typeof(NotificationPopup), new ModalParameters());
        }

        private async Task HandleInvalidSubmit()
        {
            isErrorMessageShown = true;
            await Interop.ScrollToTop(JSRuntime);
        }
    }
}