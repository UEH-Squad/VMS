﻿using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Coordinate = VMS.Application.ViewModels.Coordinate;

namespace VMS.Pages.Activities
{
    public partial class Index
    {
        private const int Take = 20;
        private List<ActivityViewModel> activityViewModels;
        private List<ActivityViewModel> filterActivities;
        private List<ActivityViewModel> activities;
        private Coordinate location;
        private bool isLoggedIn;
        private int currentPage;
        private int totalPages;

        [Inject]
        private IActivityService ActivityService { get; set; }

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private IGeoLocationService AddressLocationService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("vms.SetUserLocation");
        }

        protected async override Task OnInitializedAsync()
        {
            location = await JsRuntime.InvokeAsync<Coordinate>("vms.GetUserLocation");

            if (location is null)
            {
                string userAddresses = IdentityService.GetCurrentUserAddress();
                location = await AddressLocationService.GetCoordinateAsync(userAddresses);
            }

            activityViewModels = await ActivityService.GetAllActivitiesAsync();

            isLoggedIn = IdentityService.IsLoggedIn();

            filterActivities = activityViewModels;
            GetCurrentPage(1);
        }

        private void GetKeyword(string keyword)
        {
            filterActivities = activityViewModels.Where(a => a.Name.ToLower().Contains(keyword.Trim().ToLower())).ToList();
            GetCurrentPage(1);
        }

        private void GetFilter(FilterActivityViewModel filter)
        {
            filterActivities = activityViewModels.Where(a => a.IsVirtual == filter.Virtual || a.IsVirtual == filter.Actual)
                                                    .Where(a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.AddressPathId) || filter.AddressPathId == 0)
                                                    .Where(a => a.Organizer.Id == filter.OrgId || string.IsNullOrEmpty(filter.OrgId))
                                                    .Where(a => filter.Areas.Any(x => x.Id == a.AreaId) || filter.Areas.Count == 0)
                                                    .Where(a => filter.Skills.All(s => a.ActivitySkills.Any(x => x.SkillId == s.Id)))
                                                    .ToList();

            GetCurrentPage(1);
        }

        private void GetCurrentPage(int currentPage)
        {
            this.currentPage = currentPage;
            totalPages = filterActivities.Count / 20 + 1;
            int skip = Take * (currentPage - 1);
            activities = filterActivities.Skip(skip).Take(Take).ToList();
        }

        private void OrderActivities(ChangeEventArgs e)
        {
            switch ((string)e.Value)
            {
                case "Nearest":
                    filterActivities = filterActivities.OrderBy(a => GeoCalculator.GetDistance(location.Latitude, location.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters)).ToList();
                    GetCurrentPage(1);
                    break;

                case "Hottest":
                    filterActivities = filterActivities.OrderByDescending(a => a.MemberQuantity).ToList();
                    GetCurrentPage(1);
                    break;

                default:
                    filterActivities = filterActivities.OrderByDescending(a => a.PostDate).ToList();
                    GetCurrentPage(1);
                    break;
            }
        }

        private static double ConvertToRadian(double value)
        {
            return (Math.PI / 180) * value;
        }
    }
}