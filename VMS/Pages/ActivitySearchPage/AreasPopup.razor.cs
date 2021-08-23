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

        [Parameter]
        public List<int> ChoosenAreasList { get; set; }
        [CascadingParameter]
        private BlazoredModalInstance AreasModal { get; set; }
        [Inject]
        private IAreaService AreaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            areas = await AreaService.GetAllAreasAsync();
        }

        private void ChangeState(int id)
        {
            if (!ChoosenAreasList.Exists(a => a == id))
            {
                ChoosenAreasList.Add(id);
            }
            else
            {
                ChoosenAreasList.Remove(id);
            }
        }

        private async Task CloseModal()
        {
            await AreasModal.CloseAsync();
        }
    }
}
