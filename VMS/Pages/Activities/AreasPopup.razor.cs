﻿using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Activities
{
    public partial class AreasPopup : ComponentBase
    {
        private List<AreaViewModel> areas;

        [Parameter]
        public List<AreaViewModel> SelectedAreas { get; set; }

        [CascadingParameter]
        private BlazoredModalInstance ModalInstance { get; set; }

        [Inject]
        private IAreaService AreaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            areas = await AreaService.GetAllAreasAsync();
        }

        private void OnClickArea(AreaViewModel area)
        {
            AreaViewModel areaInSelectedAreas = SelectedAreas.FirstOrDefault(a => a.Id == area.Id);
            if (areaInSelectedAreas is null)
            {
                SelectedAreas.Add(area);
            }
            else
            {
                SelectedAreas.Remove(areaInSelectedAreas);
            }
        }

        private async Task OnClickSaveAsync()
        {
            await ModalInstance.CloseAsync();
        }
    }
}