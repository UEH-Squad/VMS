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
        private List<int> isShowSubSkills = new();

        [Parameter]
        public List<int> choosenSkillsList { get; set; }
        [Inject]
        private ISkillService SkillService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            skills = await SkillService.GetAllSkillsAsync();
        }

        private void ChangeState(int skillId)
        {
            if (!choosenSkillsList.Exists(s => s == skillId))
            {
                choosenSkillsList.Add(skillId);
            }
            else
            {
                choosenSkillsList.Remove(skillId);
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

        //Close and save button
        [CascadingParameter]
        private BlazoredModalInstance SkillsModal { get; set; }
        void CloseModal()
        {
            SkillsModal.CloseAsync();
        }

        void SaveModal()
        {
            SkillsModal.CloseAsync();
        }
    }
}
