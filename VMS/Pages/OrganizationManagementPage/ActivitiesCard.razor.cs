using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.OrganizationManagementPage
{
    public partial class ActivitiesCard : ComponentBase
    {
        private int rateId;
        private int dropdownId;
        private int confirmDeleteId;
        private int confirmCloseId;
        private int popupDelete;
        private int popupClose;
        private PaginatedList<ActivityViewModel> data = new(new(), 0, 1, 1);

        [Parameter]
        public FilterActivityViewModel Filter { get; set; }
        [Parameter]
        public bool IsSearch { get; set; } = false;
        [Parameter]
        public string SearchValue { get; set; } = "";

        [Inject]
        private IActivityService ActivityService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            data = await ActivityService.GetAllActivitiesAsync(IsSearch, SearchValue, Filter, page);
        }

        private async Task HandlePageChanged()
        {
            data = await ActivityService.GetAllActivitiesAsync(IsSearch, SearchValue, Filter, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private void ChangeRateState(int id)
        {
            rateId = (rateId == id ? 0 : id);
            dropdownId = 0;
            confirmCloseId = 0;
            confirmDeleteId = 0;
        }

        private void ChangeDropdownState(int id)
        {
            dropdownId = (dropdownId == id ? 0 : id);
            rateId = 0;
            confirmCloseId = 0;
            confirmDeleteId = 0;
        }

        private void ChangeConfirmDeleteState(int id)
        {

        }
    }
}
