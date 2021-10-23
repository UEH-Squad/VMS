using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.Organization.VolunteersListPage
{
    public partial class Index : ComponentBase
    {
        private string searchValue = string.Empty;
        [Parameter]
        public int ActId { get; set; } = 1;
        private List<int> checkList = new();
        private bool isDeletedList = false;
        private PaginatedList<ListVolunteerViewModel> pagedResult = new(new(), 0, 1, 1);
        private void OnChange(PaginatedList<ListVolunteerViewModel> paginated)
        {
            this.pagedResult = paginated;
            StateHasChanged();
            isDeletedList = !isDeletedList;
        }

        private void OnSearch(PaginatedList<ListVolunteerViewModel> paginated)
        {
            this.pagedResult = paginated;
            StateHasChanged();
        }
        private void ValueChange(string value)
        {
            this.searchValue = value;
            StateHasChanged();
        }
        private void OnChecked(List<int> checkedList)
        {
            this.checkList = checkedList;
        }
    }
}
