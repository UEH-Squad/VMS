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
using VMS.Common.Enums;

namespace VMS.Pages.Admin.ActivityManagement
{
    public partial class EditRequirement : ComponentBase
    {
        [Parameter]
        public int ActId { get; set; }
        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        private EditRequirementViewModel edit = new();
        private IReadOnlyList<IBrowserFile> selectedImages;
        private readonly string space = " ";

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IUploadService UploadService { get; set; }
        private async Task CloseModal()
        {
            await Modal.CloseAsync();
        }
        private IList<string> chosenTargets = new List<string>();
        private async Task<IEnumerable<string>> SearchTargets(string searchText)
        {
            List<string> targets = PartToFix.GetList();
            return await Task.FromResult(targets.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList());
        }
        private List<string> Image { get; set; } = new();

        private async Task AddEditRequirement()
        {
            edit.ActivityId = ActId;
            edit.IsReport = false;
            edit.IsReportUser = false;
            edit.CreateDate = DateTime.Now;
            edit.Images = Image;
            edit.PartToFix = (chosenTargets?.First());
            await ActivityService.EditRequirementAct(edit);
        }
        private bool isChangeFile = false;

        private async Task OnInputFileAsync(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            selectedImages = imageFiles;
            Image.Clear();
            isChangeFile = true;
            foreach (var file in imageFiles)
            {
                if (file.ContentType != "image/jpeg")
                {
                    StateHasChanged();
                }
                else
                {
                    string x = await UploadService.SaveImageAsync(file, "admin", ImgFolder.Activities);
                    Image.Add(x);
                }
            }
        }
    }
}
