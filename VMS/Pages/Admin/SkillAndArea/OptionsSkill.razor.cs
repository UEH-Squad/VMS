using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Admin.SkillAndArea
{
    public partial class OptionsSkill : ComponentBase
    {
        private bool isAddSuccess;
        private bool isGroupSkill;
        private bool isSkillShow;
        private string skillChoosenValue;

        private List<SkillViewModel> parentSkills = new();

        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }

        [Parameter] public bool IsAdd { get; set; }

        [Parameter] public SkillViewModel Skill { get; set; } = new();

        [Inject] private ISkillService SkillService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            parentSkills = await SkillService.GetAllSkillsAsync();

            parentSkills = parentSkills.OrderByDescending(x => x.Id == Skill.ParentSkillId).ToList();

            skillChoosenValue = Skill.ParentSkillId is null ? "Kỹ năng" : parentSkills.Find(x => x.Id == Skill.ParentSkillId).Name;

            isGroupSkill = Skill.ParentSkillId is not null; 
        }

        private void ChooseSkill(SkillViewModel skill)
        {
            Skill.ParentSkillId = skill.Id;
            skillChoosenValue = skill.Name;
        }

        private void ResetParentSkill()
        {
            Skill.ParentSkillId = null;
            skillChoosenValue = "Kỹ năng";
        }

        private void ToggleSkillDropdown()
        {
            isSkillShow = !isSkillShow;
        }

        private void CloseSkillDropdown()
        {
            isSkillShow = false;
        }

        private async Task OnValidSubmitAsync()
        {
            await SkillService.UpdateListSkillsAsync(new() { Skill });
            isAddSuccess = true;
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
