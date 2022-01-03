using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class EditRequirement : ComponentBase
    {
        [Parameter]
        public int ActId { get; set; }
        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        private EditRequirementViewModel edit = new();

        [Inject]
        private IActivityService ActivityService { get; set; }
      
        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
        private IList<string> chosenTargets = new List<string>();
        private async Task<IEnumerable<string>> SearchTargetsAsync(string searchText)
        {
            List<string> targets = PartToFix.GetList();
            return await Task.FromResult(targets.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList());
        }
        private List<string> Image { get; set; } = new();
        private void ImageChange(List<string> img)
        {
            Image = img;
        }
        private async Task AddEditRequirementAsync()
        {
            edit.ActivityId = ActId;
            edit.IsReport = false;
            edit.IsReportUser = false;
            edit.CreateDate = DateTime.Now;
            edit.Images = Image;
            edit.PartToFix = (chosenTargets?.First());
            await ActivityService.EditRequirementActAsync(edit);
        }
       
    }
}
