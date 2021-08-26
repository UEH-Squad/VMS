using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class SkillsPopup : ComponentBase
    {
        private List<SkillViewModel> skills;
        private List<int> isShowSubSkills = new();
        private List<SkillViewModel> choosenSkills;

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
            if (!isShowSubSkills.Exists(s => s == skillId))
            {
                isShowSubSkills.Add(skillId);
            }
            else
            {
                isShowSubSkills.Remove(skillId);
            }
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
