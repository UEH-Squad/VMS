using Blazored.Modal;
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
using VMS.Common.Enums;

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


        private bool isEditPage;
        private bool isLoading;
        private IList<string> chosenTargets = new List<string>();
        private readonly IList<AreaViewModel> choosenAreas = new List<AreaViewModel>();
        private bool isErrorMessageShown = false;
        private IBrowserFile uploadFile;
        [CascadingParameter] public string UserId { get; set; }
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
            isLoading = true;
            if (NavigationManager.Uri.Contains(Routes.CreateActivity))
            {
                return;
            }

            isEditPage = true;
            CreateActivityViewModel activityFromParam = await ActivityService.GetCreateActivityViewModelAsync(ActivityId);

            if (activityFromParam == null
                || !string.Equals(activityFromParam.OrgId, UserId) && !IdentityService.IsInRole(UserId, Role.Admin))
            {
                NavigationManager.NavigateTo("404");
                return;
            }

            activity = activityFromParam;
            chosenTargets = chosenTargets.Concat(activity.Targets.Split('|')).ToList();
            choosenAreas.Add(new()
            {
                Id = activity.AreaId,
                Name = activity.AreaName,
                Icon = activity.AreaIcon
            });
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                isLoading = false;
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

        private void HandleFileChanged(InputFileChangeEventArgs args)
        {
            uploadFile = args.File;
        }

        private async Task HandleImageDiscarded()
        {
            uploadFile = null;
        }

        private void OnStartDateChanged(ChangeEventArgs args)
        {
            activity.StartDate = DateTime.Parse(args.Value.ToString());
        }

        private void OnEndDateChanged(ChangeEventArgs args)
        {
            activity.EndDate = DateTime.Parse(args.Value.ToString());
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
            StateHasChanged();
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

            bool isActivityTypeChosen = activity.IsActual || activity.IsVirtual;
            activity.Targets = string.Join('|', chosenTargets);
            if (!isActivityTypeChosen || string.IsNullOrWhiteSpace(activity.Targets))
            {
                await HandleInvalidSubmit();
                return;
            }

            isErrorMessageShown = false;
            isLoading = true;
            try
            {
                activity.OrgId = string.IsNullOrEmpty(activity.OrgId) ? UserId : activity.OrgId;
                RenderFragment title;

                if (!isEditPage)
                {
                    if (uploadFile is null)
                    {
                        await HandleInvalidSubmit();
                        return;
                    }

                    activity.Banner = await UploadService.SaveImageAsync(uploadFile, activity.OrgId, ImgFolder.Activities);
                    ActivityId = await ActivityService.AddActivityAsync(activity);
                    title = succeededCreateTitle;
                }
                else
                {
                    if (uploadFile is not null)
                    {
                        string oldImageName = activity.Banner;
                        activity.Banner = await UploadService.SaveImageAsync(uploadFile, activity.OrgId, ImgFolder.Activities);
                        UploadService.RemoveImage(oldImageName);
                    }

                    await ActivityService.UpdateActivityAsync(activity, activity.Id);
                    title = succeededEditTitle;
                }

                isLoading = false;

                string redirectUrl = IdentityService.IsInRole(UserId, Role.Admin)
                                    ? $"{Routes.AdminActivityInfo}/{ActivityId}"
                                    : $"{Routes.ActivityInfo}/{ActivityId}";

                var modalParams = new ModalParameters();
                modalParams.Add("Title", title);
                modalParams.Add("CTAText", "Xem hoạt động");
                modalParams.Add("CTALink", redirectUrl);
                modalParams.Add("CancelText", "Đóng");
                await ShowModalAsync(typeof(NotificationPopup), modalParams);
                NavigationManager.NavigateTo($"{Routes.OrgProfile}/{UserId}", true);
            }
            catch (Exception ex)
            {
                isLoading = false;
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

        private async Task HandleCancel()
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Title", cancelTitle);
            modalParams.Add("CTAText", "Chắc chắn");
            modalParams.Add("CTALink", $"{Routes.UserProfile}");
            modalParams.Add("CancelText", "Hủy");
            await ShowModalAsync(typeof(NotificationPopup), modalParams);
        }
    }
}