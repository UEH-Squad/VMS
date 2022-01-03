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
        private ReportViewModel editRequest = new();

        [Parameter] public int ActId { get; set; }

        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        [CascadingParameter]
        public string CurrentUserId { get; set; }

        [Inject]
        private IReportService ReportService { get; set; }
      
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
            editRequest.IsRequest = true;

            editRequest.ReportBy = CurrentUserId;
            editRequest.ActivityId = ActId;
            editRequest.IsReportUser = false;
            editRequest.Reasons = chosenTargets.ToList();
            editRequest.Images = Image;

           await ReportService.AddReportAsync(editRequest);
        }
       
    }
}
