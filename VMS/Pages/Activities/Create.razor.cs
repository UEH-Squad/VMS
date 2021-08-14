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
    public partial class Create
    {
        private CreateActivityViewModel activity;
        private List<Area> areas;
        private List<Skill> skills;
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

        public Create()
        {
            activity = new CreateActivityViewModel();
        }

        protected async override Task OnInitializedAsync()
        {
            skills = await SkillService.GetAllSkillsAsync();
            areas = await AreaService.GetAllAreasAsync();

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
            // check area
            if (areas.FirstOrDefault(a => a.Id == activity.AreaId) is null)
            {
                return;
            }

            activity.OrgId = IdentityService.GetCurrentUserId();

            // save banner
            if (file is not null)
            {
                activity.Banner = await UploadService.SaveImageAsync(file, activity.OrgId);
            }

            await ActivityService.AddActivityAsync(activity);

            NavigationManager.NavigateTo(Routes.ActivitySearch);
        }
    }
}
