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
        private List<int> choosenAreas;

        [Parameter]
        public List<int> ChoosenAreasList { get; set; }
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

        private void ChangeState(int id)
        {
            if (!choosenAreas.Exists(a => a == id))
            {
                choosenAreas.Add(id);
            }
            else
            {
                choosenAreas.Remove(id);
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
