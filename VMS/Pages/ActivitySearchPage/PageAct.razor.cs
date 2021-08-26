using Blazored.Modal;
using Blazored.Modal.Services;
using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common;
using VMS.Domain.Models;
using Coordinate = VMS.Application.ViewModels.Coordinate;

namespace VMS.Pages.ActivitySearchPage
{
    public partial class PageAct : ComponentBase
    {
        private User currentUser;
        private int page = 1;
        private Coordinate location;
        private List<ActivityViewModel> activities;
        private IEnumerable<ActivityViewModel> featuredActivities;
        private PagedResult<ActivityViewModel> pagedResult = new() { Results = new() };

        [Parameter]
        public bool IsSearch { get; set; }
        [Parameter]
        public bool[] OrderList { get; set; }
        [Parameter]
        public string SearchValue { get; set; }
        [Parameter]
        public FilterActivityViewModel Filter { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IGeoLocationService AddressLocationService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public abstract class PagedResultBase
        {
            public int CurrentPage { get; set; }
            public int PageCount { get; set; }
            public int PageSize { get; set; }
            public int RowCount { get; set; }
            public int FirstRowOnPage => Math.Min((CurrentPage - 1) * PageSize + 1, RowCount);
            public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
        }

        public class PagedResult<T> : PagedResultBase where T : class
        {
            public List<T> Results { get; set; }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("vms.SetUserLocation");
        }

        protected override async Task OnInitializedAsync()
        {
            featuredActivities = await ActivityService.GetFeaturedActivitiesAsync();

            location = await JsRuntime.InvokeAsync<Coordinate>("vms.GetUserLocation");
            if (location is null)
            {
                string userAddresses = IdentityService.GetCurrentUserAddress();
                location = await AddressLocationService.GetCoordinateAsync(userAddresses);
            }

            currentUser = IdentityService.GetCurrentUserWithFavoritesAndRecruitments();
        }

        protected override async Task OnParametersSetAsync()
        {
            activities = await ActivityService.GetAllActivitiesAsync(IsSearch, SearchValue, Filter);
            HandleOrder();
            pagedResult = GetData();
        }

        private async Task HandlePageChanged()
        {
            pagedResult = GetData();
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

        private void HandleOrder()
        {
            if (OrderList[0] && OrderList[1] && OrderList[2])
            {
                activities = activities.OrderByDescending(a => a.PostDate)
                                            .ThenByDescending(a => a.MemberQuantity)
                                            .ThenBy(a => GeoCalculator.GetDistance(location.Latitude, location.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters))
                                            .ToList();
                return;
            }

            if (OrderList[0] && OrderList[2])
            {
                activities = activities.OrderByDescending(a => a.PostDate)
                                            .ThenByDescending(a => a.MemberQuantity)
                                            .ToList();
                return;
            }

            if (OrderList[0] && OrderList[1])
            {
                activities = activities.OrderByDescending(a => a.PostDate)
                                            .ThenBy(a => GeoCalculator.GetDistance(location.Latitude, location.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters))
                                            .ToList();
                return;
            }

            if (OrderList[1] && OrderList[2])
            {
                activities = activities.OrderByDescending(a => a.MemberQuantity)
                                            .ThenBy(a => GeoCalculator.GetDistance(location.Latitude, location.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters))
                                            .ToList();
                return;
            }

            if (OrderList[0])
            {
                activities = activities.OrderByDescending(a => a.PostDate).ToList();
            }

            if (OrderList[2])
            {
                activities = activities.OrderByDescending(a => a.MemberQuantity).ToList();
            }

            if (OrderList[1])
            {
                activities = activities.OrderBy(a => GeoCalculator.GetDistance(location.Latitude, location.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters)).ToList();
            }
        }

        private PagedResult<ActivityViewModel> GetData()
        {
            var result = new PagedResult<ActivityViewModel>();
            result.CurrentPage = page;
            result.RowCount = activities.Count;
            result.PageSize = 20;
            result.PageCount = result.RowCount / result.PageSize;
            result.Results = activities.Skip((page - 1) * result.PageSize).Take(result.PageSize).ToList();

            return result;
        }

        private void ShowModal(int id)
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

            Modal.Show<Signup>("", parameters, options);
        }
    }
}
