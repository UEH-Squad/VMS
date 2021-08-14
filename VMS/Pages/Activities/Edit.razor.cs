using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.Activities
{
    public partial class Edit
    {
        private CreateActivityViewModel activity;
        private string userId;
        private List<Skill> skills;
        private List<Area> areas;
        private List<AddressPath> provinces;
        private List<AddressPath> districts;
        private List<AddressPath> wards;
        private string message;
        private string banner;
        private IBrowserFile file;

        [Parameter]
        public string ActivityId { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IAreaService AreaService { get; set; }
        [Inject]
        private ISkillService SkillService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IUploadService UploadService { get; set; }
        [Inject]
        private IAddressService AddressService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userId = IdentityService.GetCurrentUserId();

            activity = await ActivityService.GetCreateActivityViewModelAsync(int.Parse(ActivityId));
            if (!string.IsNullOrEmpty(activity.Banner))
            {
                banner = "/img/" + activity.Banner;
            }

            skills = await SkillService.GetAllSkillsAsync();
            areas = await AreaService.GetAllAreasAsync();

            provinces = await AddressService.GetAllProvincesAsync();
            districts = await AddressService.GetAllAddressPathsByParentIdAsync(activity.ProvinceId);
            wards = await AddressService.GetAllAddressPathsByParentIdAsync(activity.DistrictId);
        }

        private void CheckboxChanged(ChangeEventArgs e, Skill skill)
        {
            Skill activitySkill = activity.Skills.FirstOrDefault(s => s.Id == skill.Id);
            if (activitySkill is null)
            {
                activity.Skills.Add(skill);
            }
            else
            {
                activity.Skills.Remove(activitySkill);
            }
        }

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            if (e.File.ContentType != "image/jpeg")
            {
                message = $"File không đúng định dạng..";
                this.StateHasChanged();
            }
            else
            {
                message = "";
                file = e.File;
                banner = await UploadService.GetDataUriAsync(file);
            }
        }

        private async Task UpdateActivityAsync()
        {
            // save banner
            if (file is not null)
            {
                if (!string.IsNullOrEmpty(activity.Banner))
                {
                    UploadService.RemoveImage(activity.Banner);
                }
                activity.Banner = await UploadService.SaveImageAsync(file, userId);
            }

            await ActivityService.UpdateActivityAsync(activity, int.Parse(ActivityId));

            NavigationManager.NavigateTo(Routes.ActivityInfo + "/" + ActivityId);
        }

        private async Task ProvinceSelectionChanged(int id)
        {
            activity.ProvinceId = id;
            activity.DistrictId = 0;
            activity.WardId = 0;
            districts = await AddressService.GetAllAddressPathsByParentIdAsync(activity.ProvinceId);
        }

        private async Task DistrictSelectionChanged(int id)
        {
            activity.DistrictId = id;
            activity.WardId = 0;
            wards = await AddressService.GetAllAddressPathsByParentIdAsync(activity.DistrictId);
        }
    }
}
