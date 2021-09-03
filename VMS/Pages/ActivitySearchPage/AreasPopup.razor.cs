using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class AreasPopup : ComponentBase
    {
        private List<AreaViewModel> areas;
        private List<AreaViewModel> choosenAreas;

        [Parameter]
        public List<AreaViewModel> ChoosenAreasList { get; set; }
        [CascadingParameter]
        private BlazoredModalInstance AreasModal { get; set; }

        [Inject]
        private IAreaService AreaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            areas = await AreaService.GetAllAreasAsync();
            choosenAreas = new();
            choosenAreas.AddRange(ChoosenAreasList);
        }

        private void ChangeState(AreaViewModel areaChoosen)
        {
            AreaViewModel area = choosenAreas.Find(a => a.Id == areaChoosen.Id);
            if (area is null)
            {
                choosenAreas.Add(areaChoosen);
            }
            else
            {
                choosenAreas.Remove(area);
            }
        }

        private async Task CloseModalAsync()
        {
            await AreasModal.CloseAsync();
        }

        private async Task SaveModalAsync()
        {
            ChoosenAreasList.Clear();
            ChoosenAreasList.AddRange(choosenAreas);
            await AreasModal.CloseAsync();
        }
    }
}