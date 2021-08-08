using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class UserWithActService : BaseService, IUserWithActService
    {
        public UserWithActService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }
        public async Task<List<UserWithActivityViewModel>> GetActivitiesAsync()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            List<Activity> activities = await _repository.GetListAsync<Activity>(context);

            IEnumerable<ActivityViewModel> Activities = activities.Select(x => new ActivityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                MemberQuantity = x.MemberQuantity
            }
            );

            //Seeding to user loacation
            var user = new UserViewModel();
            user.Lat = 14.155555;
            user.Long = 125.124444;

            List<ActivityViewModel> activitiesList = Activities.ToList();

            IEnumerable<UserWithActivityViewModel> userWithActivity = activitiesList.Select(x => new UserWithActivityViewModel
            {
                ActivityId = x.Id,
                Name = x.Name,
                Distance = Haversine(user,x.Latitude,x.Longitude),
                MemberQuantity = x.MemberQuantity
            });

            return userWithActivity.OrderBy(x => x.Distance).ToList();
        }
        static double Haversine(UserViewModel user, double latitude, double longitude) 
        {
            double latDistance = (Math.PI / 180) * (latitude - user.Lat);
            double longDistance = (Math.PI / 180) * (longitude - user.Long);

            double userLatRadian = (Math.PI / 180) * user.Lat;
            double actLatRadian = (Math.PI / 180) * latitude;

            double formula = Math.Pow(Math.Sin(latDistance / 2), 2) + Math.Pow(Math.Sin(longDistance / 2), 2) * Math.Cos(userLatRadian) * Math.Cos(actLatRadian);

            double distance = Math.Round(2 * 6371 * Math.Asin(Math.Sqrt(formula)), 2);

            return distance;
            /* Source: https://www.geeksforgeeks.org/haversine-formula-to-find-distance-between-two-points-on-a-sphere/ */
        }

        public async Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithUserLocAsync()
        {
            List<UserWithActivityViewModel> nearestActivities = await GetActivitiesAsync();
            List<UserWithActivityViewModel> newestActivities = nearestActivities.Take(4).ToList();

            return newestActivities.OrderByDescending(x => x.ActivityId).ToList();
        }

        public async Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithUserLocAsync()
        {
            List<UserWithActivityViewModel> nearestActivities = await GetActivitiesAsync();
            List<UserWithActivityViewModel> featuredtActivities = nearestActivities.Take(4).ToList();

            return featuredtActivities.OrderByDescending(x => x.MemberQuantity).ToList();
        }

        public async Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithoutUserLocAsync()
        {
            List<UserWithActivityViewModel> newestActivities = await GetActivitiesAsync();

            return newestActivities.OrderByDescending(x => x.ActivityId).Take(4).ToList();
        }
        public async Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithoutUserLocAsync()
        {
            List<UserWithActivityViewModel> featuredActivities = await GetActivitiesAsync();

            return featuredActivities.OrderByDescending(x => x.MemberQuantity).Take(4).ToList();
        }


    }
}
