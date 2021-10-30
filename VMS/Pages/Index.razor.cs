using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Common.Extensions;

namespace VMS.Pages
{
    public partial class Index : ComponentBase
    {
        private List<ActivityViewModel> newestActivities, featuredActivities;

        [Inject] private IActivityService ActivityService { get; set; }
        [Inject] private IIdentityService IdentityService { get; set; }
        [Inject] private IGeoLocationService AddressLocationService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("vms.SetUserLocation");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var userLocation = await JsRuntime.InvokeAsync<CoordinateJs>("vms.GetUserLocation");
            Coordinate userCoordinate = userLocation.ToCoordinate();
            if (userCoordinate is null)
            {
                string userAddresses = IdentityService.GetCurrentUserAddress();
                userCoordinate = await AddressLocationService.GetCoordinateAsync(userAddresses);
            }

            Dictionary<ActOrderBy, bool> orderList = new(new List<KeyValuePair<ActOrderBy, bool>>() {
                new KeyValuePair<ActOrderBy, bool>(ActOrderBy.Newest, true),
                new KeyValuePair<ActOrderBy, bool>(ActOrderBy.Nearest, true),
                new KeyValuePair<ActOrderBy, bool>(ActOrderBy.Hottest, false)
            });

            newestActivities = (await ActivityService.GetAllActivitiesAsync(orderList, userCoordinate)).Items;

            orderList[ActOrderBy.Newest] = false;
            orderList[ActOrderBy.Hottest] = true;

            featuredActivities = (await ActivityService.GetAllActivitiesAsync(orderList, userCoordinate)).Items;
        }
    }
}
