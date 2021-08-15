using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            activity = await ActivityService.GetCreateActivityViewModelAsync(int.Parse(ActivityId));
            skills = await SkillService.GetAllSkillsAsync();
            areas = await AreaService.GetAllAreasAsync();
            userId = IdentityService.GetCurrentUserId();
            if (!string.IsNullOrEmpty(activity.Banner))
            {
                banner = "/img/" + activity.Banner;
            }
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
            // check area
            if (areas.FirstOrDefault(a => a.Id == activity.AreaId) is null)
            {
                return;
            }

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

            NavigationManager.NavigateTo(Routes.ActivitySearch + "/" + ActivityId);
        }
    }
}
