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
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private ISkillService SkillService { get; set; }

        private readonly Fake activity = new()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7)
        };

        private readonly List<string> targets = new()
        {
            "Sinh viên năm nhất",
            "Sinh viên năm hai",
            "Sinh viên năm ba",
            "Tất cả mọi đối tượng"
        };

        private void OnAddressChanged(int provinceId, int districtId, int wardId)
        {
            activity.ProvinceId = provinceId;
            activity.DistrictId = districtId;
            activity.WardId = wardId;
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
            parameters.Add("ChoosenAreasList", activity.Areas);

            await ShowModalAsync(typeof(ActivitySearchPage.AreasPopup), parameters);
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
    }

    public class Fake : Activity
    {
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public IList<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
        public IList<AreaViewModel> Areas { get; set; } = new List<AreaViewModel>();
        public IList<string> Targets { get; set; }
    }
}