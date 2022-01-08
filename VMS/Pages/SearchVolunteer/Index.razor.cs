using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Pages.SearchVolunteer
{
    public partial class Index : ComponentBase
    {
        private FilterVolunteerViewModel filter = new();
        private PaginatedList<UserViewModel> pagedResult = new(new(), 0, 1, 1);

        [Inject] private IUserService UserService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await OnPageChangedAsync(1);
        }

        private async Task OnFilterChangedAsync(FilterVolunteerViewModel filter)
        {
            this.filter = filter;
            await OnPageChangedAsync(1);
        }

        private async Task OnPageChangedAsync(int page)
        {
            pagedResult = await UserService.GetAllVolunteers(filter, page);
        }
    }
}
