using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Pages.Activities
{
    public partial class Create
    {
        private CreateActivityViewModel activity;
        private List<Area> areas;
        private List<Skill> skills;
        private List<AddressPath> provinces;
        private List<AddressPath> districts;
        private List<AddressPath> wards;
        private string message;
        private string banner;
        private IBrowserFile file;

        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IAreaService AreaService { get; set; }
        [Inject]
        private ISkillService SkillService { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IUploadService UploadService { get; set; }
        [Inject]
        private IAddressService AddressService { get; set; }

        public Create()
        {
            activity = new CreateActivityViewModel();
        }

        protected async override Task OnInitializedAsync()
        {
            skills = await SkillService.GetAllSkillsAsync();
            areas = await AreaService.GetAllAreasAsync();
            provinces = await AddressService.GetAllProvincesAsync();

            // init collection for activity
            activity.Skills = new List<Skill>();
        }

        private void CheckboxChanged(ChangeEventArgs e, Skill skill)
        {
            if (!activity.Skills.Contains(skill))
            {
                activity.Skills.Add(skill);
            }
            else
            {
                activity.Skills.Remove(skill);
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

        private async Task AddActivityAsync()
        {
            activity.OrgId = IdentityService.GetCurrentUserId();

            AddressPath addressPath = await AddressService.GetAddressPathByIdAsync(activity.WardId);
            activity.FullAddress = $"{activity.Address}, {addressPath.Name}, {addressPath.PreviousPath.Name}, {addressPath.PreviousPath.PreviousPath.Name}";

            // save banner
            if (file is not null)
            {
                activity.Banner = await UploadService.SaveImageAsync(file, activity.OrgId);
            }

            await ActivityService.AddActivityAsync(activity);

            NavigationManager.NavigateTo(Routes.Topic);
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
