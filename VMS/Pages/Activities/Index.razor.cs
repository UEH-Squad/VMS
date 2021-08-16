using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Activities
{
    public partial class Index
    {
        private List<ActivityViewModel> activityViewModels;
        private List<ActivityViewModel> activities;
        private bool isLoggedIn;
        private CoordinateResponse userLocation;
        private int currentPage = 1;
        private int totalPages = 10;

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        [Inject]
        private IAddressLocationService AddressLocationService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("vms.SetUserLocation");
        }

        protected async override Task OnInitializedAsync()
        {   
            userLocation = await JsRuntime.InvokeAsync<CoordinateResponse>("vms.GetUserLocation");

            if (userLocation is null)
            {
                string userAddresses = IdentityService.GetCurrentUserAddress();
                userLocation = await AddressLocationService.GetCoordinateAsync(userAddresses);
            }

            activityViewModels = await ActivityService.GetAllActivitiesAsync();

            activities = activityViewModels;

            isLoggedIn = IdentityService.IsLoggedIn();
        }

        private void GetFilter(FilterActivityViewModel filter)
        {
            activities = activityViewModels.Where(a => a.IsVirtual == filter.Virtual || a.IsVirtual == filter.Actual)
                                            .Where(a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.AddressPathId) || filter.AddressPathId == 0)
                                            .Where(a => a.Organizer.Id == filter.OrgId || string.IsNullOrEmpty(filter.OrgId))
                                            .Where(a => filter.Areas.Any(x => x.Id == a.AreaId) || filter.Areas.Count == 0)
                                            .Where(a => filter.Skills.All(s => a.ActivitySkills.Any(x => x.SkillId == s.Id)))
                                            .ToList();
        }

        private void GetCurrentPage(int currentPage)
		{
            this.currentPage = currentPage;
		}

        private void OrderActivities(ChangeEventArgs e)
        {
            switch ((string)e.Value)
            {
                case "Nearest":
                    activities = activities.OrderBy(a => Distance(userLocation, a.Coordinate)).ToList();
                    break;

                case "Hottest":
                    activities = activities.OrderByDescending(a => a.MemberQuantity).ToList();
                    break;

                default:
                    activities = activities.OrderByDescending(a => a.PostDate).ToList();
                    break;
            }
        }

        /* source: https://gist.github.com/jammin77/033a332542aa24889452 */
        private double Distance(CoordinateResponse userPosition, CoordinateResponse activityPosition)
        {
            double dLat = ConvertToRadian(userPosition.Lat - activityPosition.Lat);
            double dLon = ConvertToRadian(userPosition.Long - activityPosition.Long);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ConvertToRadian(userPosition.Lat)) *
                        Math.Cos(ConvertToRadian(activityPosition.Lat)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

            return 6371 * c;
        }

        private double ConvertToRadian(double value)
        {
            return (Math.PI / 180) * value;
        }
    }
}
