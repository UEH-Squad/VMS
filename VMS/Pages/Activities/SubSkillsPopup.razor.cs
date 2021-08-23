using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Pages.Activities
{
    public partial class SubSkillsPopup : ComponentBase
    {
        private List<Skill> skills;

        [Parameter]
        public List<Skill> SelectedSkills { get; set; }

        [Parameter]
        public int ParentSkillId { get; set; }

        [CascadingParameter]
        private BlazoredModalInstance ModalInstance { get; set; }

        [Inject]
        private ISkillService SkillService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            skills = await SkillService.GetAllSubSkillsAsync(ParentSkillId);
        }

        private void OnClickSkill(Skill skill)
        {
            Skill skillInSelectedSkills = SelectedSkills.FirstOrDefault(s => s.Id == skill.Id);
            if (skillInSelectedSkills is null)
            {
                SelectedSkills.Add(skill);
            }
            else
            {
                SelectedSkills.Remove(skillInSelectedSkills);
            }
        }
    }
}