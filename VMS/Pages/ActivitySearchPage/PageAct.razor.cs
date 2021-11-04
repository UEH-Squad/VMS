using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Common.Enums;
using VMS.Common.Extensions;
using VMS.Domain.Models;
using VMS.GenericRepository;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class PageAct : ComponentBase
    {
        private User currentUser;
        private int page = 1;
        private Coordinate userCoordinate;
        private IEnumerable<ActivityViewModel> featuredActivities;
        private PaginatedList<ActivityViewModel> pagedResult = new(new(), 0, 1, 1);

        [Parameter]
        public Dictionary<ActOrderBy, bool> OrderList { get; set; }
        [Parameter]
        public FilterActivityViewModel Filter { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }
        [CascadingParameter]
        public string CurrentUserId { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IGeoLocationService AddressLocationService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("vms.SetUserLocation");
        }

        protected override async Task OnInitializedAsync()
        {
            featuredActivities = await ActivityService.GetFeaturedActivitiesAsync();

            var userLocation = await JsRuntime.InvokeAsync<CoordinateJs>("vms.GetUserLocation");
            userCoordinate = userLocation.ToCoordinate();
            if (userCoordinate is null)
            {
                string userAddresses = IdentityService.GetCurrentUserAddress();
                userCoordinate = await AddressLocationService.GetCoordinateAsync(userAddresses);
            }

            currentUser = IdentityService.GetUserWithFavoritesAndRecruitmentsById(CurrentUserId);
        }

        protected override async Task OnParametersSetAsync()
        {
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, 1, OrderList, userCoordinate);
        }

        private async Task HandlePageChanged()
        {
            pagedResult = await ActivityService.GetAllActivitiesAsync(Filter, page, OrderList, userCoordinate);
            StateHasChanged();
            await Interop.ScrollToTop(JsRuntime);
        }

        private void HandleFavorite(int id)
        {
            Favorite favorite = currentUser.Favorites.FirstOrDefault(f => f.ActivityId == id);

            if (favorite is null)
            {
                currentUser.Favorites.Add(new()
                {
                    UserId = currentUser.Id,
                    ActivityId = id,
                    CreatedDate = DateTime.Now
                });
            }
            else
            {
                currentUser.Favorites.Remove(favorite);
            }

            IdentityService.UpdateUser(currentUser);
        }

        private async Task ShowSignupAsync(int id)
        {
            ModalParameters parameters = new();
            parameters.Add("ActivityId", id);
            parameters.Add("CurrentUser", currentUser);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };

            await Modal.Show<Signup>("", parameters, options).Result;
        }

        private void ShowRequireSignup()
        {

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };

            Modal.Show<Shared.Components.RequireSignup>("", options);
        }

        private bool IsSignupTimeExpired(ActivityViewModel activity)
        {
            return currentUser.Recruitments.Any(f => f.ActivityId == activity.Id) || !(activity.OpenDate <= DateTime.Now && DateTime.Now.Date <= activity.CloseDate) || activity.IsClosed;
        }
    }
}
