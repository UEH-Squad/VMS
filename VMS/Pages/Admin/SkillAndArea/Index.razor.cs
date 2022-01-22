using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;

namespace VMS.Pages.Admin.SkillAndArea
{
    public partial class Index : ComponentBase
    {
        private bool isArea = true;
        private List<AreaViewModel> chosenAreas = new();

        [Inject] private IAreaService AreaService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        private void OnChooseType(bool isArea)
        {
            this.isArea = isArea;
        }

        private async Task ShowModalAddVideoAsync()
        {
            await Modal.Show<AddVideo>("", BlazoredModalOptions.GetModalOptions()).Result;
        }

        private async Task ShowOptionAsync()
        {

            if (isArea)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(OptionsArea.IsAdd), true);
                await Modal.Show<OptionsArea>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;
            }
            else
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(OptionsSkill.IsAdd), true);
                Modal.Show<OptionsSkill>("", parameters, BlazoredModalOptions.GetModalOptions());
            }

            Refresh();
        }

        private void OnChosenListChanged(List<AreaViewModel> chosenList)
        {
            chosenAreas = chosenList;
        }

        private async Task ShowPopupDeleteAsync()
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(ShowDelete.isArea), isArea);

            var result = await Modal.Show<ShowDelete>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            if ((bool)result.Data)
            {
                if (isArea)
                {
                    chosenAreas.ForEach(x => x.IsDeleted = true);
                    await AreaService.UpdateListAreasAsync(chosenAreas);
                    Refresh();
                }
                else
                {

                }
            }
        }

        private void Refresh()
        {
            NavigationManager.NavigateTo(Routes.AdminFeatureSuggestionManagement, true);
        }
    }
}
