using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Pages.Activities
{
	public partial class SkillsPopup
	{
		private List<Skill> skills;

		[Parameter]
		public List<Skill> SelectedSkills { get; set; }
        [CascadingParameter]
		private BlazoredModalInstance ModalInstance { get; set; }
		[Inject]
		private ISkillService SkillService { get; set; }
		[Inject]
		private IModalService ModalService { get; set; }

		protected async override Task OnInitializedAsync()
		{
			skills = await SkillService.GetAllSkillsAsync();
		}

		private async Task OnClickSkill(Skill skill)
		{
			if (skill.SubSkills is null)
            {
				AddSkill(skill);
            }
            else
            {
				ModalParameters parameters = new ModalParameters();
				parameters.Add("SelectedSkills", SelectedSkills);
				parameters.Add("ParentSkillId", skill.Id);

				var messageForm = ModalService.Show<SubSkillsPopup>("", parameters);
				ModalResult result = await messageForm.Result;
			}
		}

		private void AddSkill(Skill skill)
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

		private async Task OnClickSaveAsync()
		{
			await ModalInstance.CloseAsync();
		}
	}
}
