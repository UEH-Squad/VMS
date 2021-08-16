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
            var filterActivities = activityViewModels.Where(a => a.IsVirtual == filter.Virtual || a.IsVirtual == filter.Actual);

            if (!string.IsNullOrEmpty(filter.OrgId))
            {
                filterActivities = filterActivities.Where(a => a.Organizer.Id == filter.OrgId);
            }

            if (filter.ProvinceId != 0)
            {
                filterActivities = filterActivities.Where(a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.ProvinceId));

                if (filter.DistrictId != 0)
                {
                    filterActivities = filterActivities.Where(a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.DistrictId));
                }
            }

            if (filter.Areas.Count != 0)
            {
                filterActivities = filterActivities.Where(a => filter.Areas.Any(x => x.Id == a.Area.Id));
            }

            if (filter.Skills.Count != 0)
            {
                filterActivities = filterActivities.Where(a => filter.Skills
                                                            .All(s => a.ActivitySkills
                                                            .Any(x => x.SkillId == s.Id)));
            }

            activities = filterActivities.ToList();
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
