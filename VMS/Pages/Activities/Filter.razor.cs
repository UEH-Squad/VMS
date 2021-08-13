using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Pages.Activities
{
    public partial class Filter
    {
        private List<Skill> skills;
        private List<Area> areas;
        private List<User> organizers;
        private FilterActivityViewModel filter;

        [Parameter]
        public EventCallback<FilterActivityViewModel> FilterEventCallback { get; set; }
        [Inject]
        private ISkillService SkillService { get; set; }
        [Inject]
        private IAreaService AreaService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IModalService ModalService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            filter = new FilterActivityViewModel();
            skills = await SkillService.GetAllSkillsAsync();
            areas = await AreaService.GetAllAreasAsync();
            organizers = IdentityService.GetAllOrganizers();
        }

        private async Task FilterAsync()
        {
            await FilterEventCallback.InvokeAsync(filter);
        }

        private void SkillsCheckboxChanged(ChangeEventArgs e, Skill skill)
        {
            if (!filter.Skills.Contains(skill))
            {
                filter.Skills.Add(skill);
            }
            else
            {
                filter.Skills.Remove(skill);
            }
        }

        private async Task ShowAreasPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add("SelectedAreas", filter.Areas);
            var messageForm = ModalService.Show<AreasPopup>("", parameters);
            var result = await messageForm.Result;
            if (result.Data is not null)
            {
                filter.Areas = (List<Area>)result.Data;
            }
        }
    }
}
