using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Admin.SkillAndArea
{
    public partial class OptionsArea : ComponentBase
    {
        private bool isAddSuccess;
        private List<AreaViewModel> pinnedAreas = new();

        [Parameter] public AreaViewModel Area { get; set; } = new();
        [Parameter] public bool IsAdd { get; set; }

        [CascadingParameter]
        public BlazoredModalInstance Modal { get; set; }

        [Inject] IAreaService AreaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            pinnedAreas = await AreaService.GetAllAreasAsync(true);

            var existArea = pinnedAreas.Find(x => x.Id == Area.Id);
            if (existArea != null)
            {
                pinnedAreas.Remove(existArea);
            }
        }

        private bool IsValidPinnedArea()
        {
            return pinnedAreas.Count < 4 || !Area.IsPinned;
        }

        private async Task OnValidSubmitAsync()
        {
            if (pinnedAreas.Where(x => x.IsPinned).Count() < 4)
            {
                if (IsAdd)
                {
                    await AreaService.AddAreaAsync(Area);
                }
                else
                {
                    pinnedAreas.Add(Area);
                }

                await AreaService.UpdateListAreasAsync(pinnedAreas);

                isAddSuccess = true;
            }
        }

        private async Task CloseModalAsync()
        {
            await Modal.CloseAsync();
        }
    }
}
