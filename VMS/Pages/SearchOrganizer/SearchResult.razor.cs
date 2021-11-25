using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.GenericRepository;

namespace VMS.Pages.SearchOrganizer
{
    public partial class SearchResult 
    {
        [Inject]
        private IOrganizationService OrganizationService { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        private int page = 1;
        private string userId;
        private PaginatedList<UserViewModel> pagedResult = new(new(), 0, 1, 1);

        protected override async Task OnInitializedAsync()
        {
            userId = IdentityService.GetCurrentUserId();
        }

        protected override async Task OnParametersSetAsync()
        {
            page = 1;
            pagedResult = await OrganizationService.GetAllOrgsAsync(userId, page);
        }

        private async Task HandlePageChangedAsync()
        {
            pagedResult = await OrganizationService.GetAllOrgsAsync(userId, page);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }
    }
}
