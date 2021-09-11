using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class SkillsPopup : ComponentBase
    {
        private List<SkillViewModel> skills;
        private List<SkillViewModel> choosenSkills;
        private int showSubSkillsId = 0;

        [Parameter]
        public List<SkillViewModel> ChoosenSkillsList { get; set; }
        [CascadingParameter]
        private BlazoredModalInstance SkillsModal { get; set; }
        [Inject]
        private ISkillService SkillService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            skills = await SkillService.GetAllSkillsAsync();
            choosenSkills = new();
            choosenSkills.AddRange(ChoosenSkillsList);
        }

        private void ChangeState(SkillViewModel skill)
        {
            SkillViewModel skillViewModel = choosenSkills.Find(s => s.Id == skill.Id);
            if (skillViewModel is null)
            {
                choosenSkills.Add(skill);
            }
            else
            {
                choosenSkills.Remove(skillViewModel);
            }
        }

        private void HandleShowSubSkill(int skillId)
        {
            showSubSkillsId = (showSubSkillsId == skillId ? 0 : skillId);
        }

        private async Task CloseModalAsync()
        {
            await SkillsModal.CloseAsync();
        }

        private async Task SaveModalAsync()
        {
            ChoosenSkillsList.Clear();
            ChoosenSkillsList.AddRange(choosenSkills);
            await SkillsModal.CloseAsync();
        }
    }
}
