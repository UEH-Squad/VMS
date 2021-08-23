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

        [Parameter]
        public List<SkillViewModel> ChoosenSkillsList { get; set; }
        [CascadingParameter]
        private BlazoredModalInstance SkillsModal { get; set; }
        [Inject]
        private ISkillService SkillService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            skills = await SkillService.GetAllSkillsAsync();
        }

        private void ChangeState(SkillViewModel choosenSkill)
        {
            SkillViewModel skill = ChoosenSkillsList.Find(s => s.Id == choosenSkill.Id);
            if (skill is null)
            {
                ChoosenSkillsList.Add(choosenSkill);
            }
            else
            {
                ChoosenSkillsList.Remove(choosenSkill);
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

        private async Task CloseModal()
        {
            await SkillsModal.CloseAsync();
        }

        private async Task SaveModal()
        {
            await SkillsModal.CloseAsync();
        }
    }
}
