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

		protected async override Task OnInitializedAsync()
		{
			skills = await SkillService.GetAllSkillsAsync();
		}

		private void OnClickArea(Skill skill)
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
			await ModalInstance.CloseAsync(ModalResult.Ok(SelectedSkills));
		}
	}
}
