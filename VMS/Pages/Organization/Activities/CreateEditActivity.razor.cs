﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
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

        [Parameter] public int ActivityId { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ILogger<CreateEditActivity> Logger { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private ISkillService SkillService { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }

        [Inject]
        private IUploadService UploadService { get; set; }

        private bool isEdit;
        private bool isLoading;
        private IList<string> chosenTargets = new List<string>();
        private readonly IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private bool isErrorMessageShown = false;
        private IBrowserFile uploadFile;

        private CreateActivityViewModel activity = new();

        private readonly List<string> targets = new()
        {
            "Năm nhất",
            "Năm hai",
            "Năm ba",
            "Năm tư",
            "Tất cả mọi đối tượng"
        };

        protected override async Task OnInitializedAsync()
        {
            if (ActivityId <= 0)
            {
                return;
            }

            isEdit = true;
            CreateActivityViewModel activityFromParam = await ActivityService.GetCreateActivityViewModelAsync(ActivityId);
            if (activityFromParam == null)
            {
                NavigationManager.NavigateTo("404");
            }
            else
            {
                activity = activityFromParam;
                choosenAreas.Add(new()
                {
                    Id = activity.AreaId,
                    Name = activity.AreaName,
                    Icon = activity.AreaIcon
                });

                chosenTargets = chosenTargets.Concat(activity.Targets.Split('|')).ToList();
            }
        }

        private async Task OnAddressChanged(int provinceId, string province, int districtId, string district, int wardId, string ward, string fullAddress)
        {
            activity.ProvinceId = provinceId;
            activity.Province = province;
            activity.DistrictId = districtId;
            activity.District = district;
            activity.WardId = wardId;
            activity.Ward = ward;
            activity.FullAddress = fullAddress;
        }

        private async Task HandleFileChanged(InputFileChangeEventArgs file)
        {
            uploadFile = file.File;
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
            isLoading = true;
            try
            {
                string currentUserId = IdentityService.GetCurrentUserId();

                if (ActivityId <= 0)
                {
                    if (uploadFile is null)
                    {
                        await HandleInvalidSubmit();
                        return;
                    }

                    activity.Banner = await UploadService.SaveImageAsync(uploadFile, currentUserId);
                    await ActivityService.AddActivityAsync(activity);
                }
                else
                {
                    if (uploadFile is not null)
                    {
                        UploadService.RemoveImage(activity.Banner);
                        activity.Banner = await UploadService.SaveImageAsync(uploadFile, currentUserId);
                    }

                    await ActivityService.UpdateActivityAsync(activity, activity.Id);
                }

                isLoading = false;

                var modalParams = new ModalParameters();
                modalParams.Add("IsEdit", isEdit);
                modalParams.Add("CTALink", $"{Routes.EditActivity}/{ActivityId}");
                await ShowModalAsync(typeof(NotificationPopup), modalParams);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurs when trying to create/edit activity", ex.Message);
                await JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        private async Task HandleInvalidSubmit()
        {
            isLoading = false;
            isErrorMessageShown = true;
            await Interop.ScrollToTop(JSRuntime);
        }
    }
}