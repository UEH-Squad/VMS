using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.Admin.SkillAndArea
{
    public partial class ListSkills : ComponentBase
    {
        private bool isLoading;
        private PaginatedList<SkillViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter] public bool ShowCheckbox { get; set; }

        [Parameter] public List<SkillViewModel> ChosenSkills { get; set; } = new();

        [Parameter] public EventCallback<List<SkillViewModel>> ChosenListChanged { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject] private ISkillService SkillService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await SetDataAsync();
        }

        private async Task SetDataAsync(int page = 1)
        {
            isLoading = true;
            pagedResult = await SkillService.GetAllSkillsAsync(page);
            isLoading = false;
        }

        private async Task HandlePageChangedAsync(int page)
        {
            await SetDataAsync(page);
            StateHasChanged();
            await JS.InvokeVoidAsync("window.scrollTo", 0, 0);
        }

        private async Task ShowEditModalAsync(SkillViewModel skill)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(OptionsSkill.IsAdd), false);
            parameters.Add(nameof(OptionsSkill.Skill), skill);

            await Modal.Show<OptionsSkill>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            await SetDataAsync();
        }

        private async Task OnChooseSkillAsync(SkillViewModel skill)
        {
            var existSkill = ChosenSkills.Find(x => x.Id == skill.Id);

            if (existSkill is null)
            {
                ChosenSkills.Add(skill);
            }
            else
            {
                ChosenSkills.Remove(skill);
            }

            if (skill.SubSkills is not null && skill.SubSkills.Count > 0)
            {
                foreach (var subSkill in skill.SubSkills)
                {
                    await OnChooseSkillAsync(subSkill);
                }
            }

            await OnChosenListChangedAsync();
        }

        private async Task OnChosenListChangedAsync()
        {
            await ChosenListChanged.InvokeAsync(ChosenSkills);
        }
    }
}
