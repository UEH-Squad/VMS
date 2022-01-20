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
    public partial class ListArea : ComponentBase
    {
        private bool isLoading;
        private List<AreaViewModel> chosenList; 
        private PaginatedList<AreaViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter] public EventCallback<List<AreaViewModel>> ChosenListChanged { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        [Inject] private IAreaService AreaService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetDataAsync();
        }

        private async Task SetDataAsync(int page = 1)
        {
            isLoading = true;

            pagedResult = await AreaService.GetAllAreasAsync(page);

            chosenList = new();
            await OnChosenListChangedAsync();

            isLoading = false;
        }

        private async Task HandlePageChangedAsync(int page)
        {
            await SetDataAsync(page);
            StateHasChanged();
            await JS.InvokeVoidAsync("window.scrollTo", 0, 0);
        }

        private async Task ShowEditModalAsync(AreaViewModel area)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(OptionsArea.IsAdd), false);
            parameters.Add(nameof(OptionsArea.Area), area);

            await Modal.Show<OptionsArea>("", parameters, BlazoredModalOptions.GetModalOptions()).Result;

            await SetDataAsync();
        }

        private async Task OnChooseAreaAsync(AreaViewModel area)
        {
            var existArea = chosenList.Find(x => x.Id == area.Id);

            if (existArea is null)
            {
                chosenList.Add(area);
            }
            else
            {
                chosenList.Remove(area);
            }

            await OnChosenListChangedAsync();
        }

        private async Task OnChosenListChangedAsync()
        {
            await ChosenListChanged.InvokeAsync(chosenList);
        }
    }
}
