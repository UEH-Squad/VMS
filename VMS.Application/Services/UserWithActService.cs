using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class UserWithActService : BaseService, IUserWithActService
    {
        protected readonly IIdentityService _identityService;
        public UserWithActService(IIdentityService identityService, IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
            _identityService = identityService;
        }
        public async Task<List<UserWithActivityViewModel>> GetActivitiesWithUserLocAsync(double Lat, double Long)
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

            List<ActivityViewModel> activitiesList = Activities.ToList();
            IEnumerable<UserWithActivityViewModel> userWithActivity = activitiesList.Select(x => new UserWithActivityViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                Distance = Haversine(Lat, Long, x.Latitude, x.Longitude),
                MemberQuantity = x.MemberQuantity
            });
            return userWithActivity.OrderBy(x => x.Distance).ToList();
        }
        static double Haversine(double userLat, double userLong, double activityLat, double activityLon)
        {
            double latDistance = (Math.PI / 180) * (activityLat - userLat);
            double longDistance = (Math.PI / 180) * (activityLon - userLong);
            double userLatRadian = (Math.PI / 180) * userLat;
            double actLatRadian = (Math.PI / 180) * activityLat;
            double formula = Math.Pow(Math.Sin(latDistance / 2), 2) + Math.Pow(Math.Sin(longDistance / 2), 2) * Math.Cos(userLatRadian) * Math.Cos(actLatRadian);
            double distance = Math.Round(2 * 6371 * Math.Asin(Math.Sqrt(formula)), 2);
            return distance;
            /* Source: https://www.geeksforgeeks.org/haversine-formula-to-find-distance-between-two-points-on-a-sphere/ */
        }

        public async Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithUserLocAsync(double Lat, double Long)
        {
            List<UserWithActivityViewModel> nearestActivities = await GetActivitiesWithUserLocAsync(Lat, Long);
            List<UserWithActivityViewModel> newestActivities = nearestActivities.Take(4).ToList();
            return newestActivities.OrderByDescending(x => x.ActivityId).ToList();
        }

        public async Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithUserLocAsync(double Lat, double Long)
        {
            List<UserWithActivityViewModel> nearestActivities = await GetActivitiesWithUserLocAsync(Lat, Long);
            List<UserWithActivityViewModel> featuredtActivities = nearestActivities.Take(4).ToList();
            return featuredtActivities.OrderByDescending(x => x.MemberQuantity).ToList();
        }



        public async Task<List<UserWithActivityViewModel>> GetActivitiesWithoutUserLocAsync()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            List<Activity> activities = await _repository.GetListAsync<Activity>(context);
            IEnumerable<UserWithActivityViewModel> userWithActivity = activities.Select(x => new UserWithActivityViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                MemberQuantity = x.MemberQuantity
            }
            );
            return userWithActivity.ToList();
        }
        public async Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithoutUserLocAsync()
        {
            List<UserWithActivityViewModel> newestActivities = await GetActivitiesWithoutUserLocAsync();
            return newestActivities.OrderByDescending(x => x.ActivityId).Take(4).ToList();
        }
        public async Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithoutUserLocAsync()
        {
            List<UserWithActivityViewModel> featuredActivities = await GetActivitiesWithoutUserLocAsync();
            return featuredActivities.OrderByDescending(x => x.MemberQuantity).Take(4).ToList();
        }
        public async Task<List<UserWithActivityViewModel>> GetActivitiesWithDistristAsync()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Activity> activitySpec = new()
            {
                Includes = a => a.Include(x => x.ActivityAddresses)
            };
            List<Activity> activities = await _repository.GetListAsync<Activity>(context, activitySpec);
            User user = _identityService.GetCurrentUserWithAddresses();
            ICollection<UserAddress> userAddress = user.UserAddresses;
            var userDistrict = userAddress.FirstOrDefault(x => x.AddressPath.Depth == 2);
            List<Activity> activityList = new List<Activity>();
            foreach(var act in activities)
            {
                ActivityAddress activityAddress = act.ActivityAddresses.FirstOrDefault(x => x.AddressPathId == userDistrict.AddressPathId);
                if (activityAddress != null)
                {
                    activityList.Add(act);
                }
            }
            IEnumerable<UserWithActivityViewModel> userWithActivity = activityList.Select(x => new UserWithActivityViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                MemberQuantity = x.MemberQuantity
            }
            );
            return userWithActivity.ToList();
        }
        public async Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithDistristAsync()
        {
            List<UserWithActivityViewModel> newestActivities = await GetActivitiesWithDistristAsync();
            return newestActivities.OrderByDescending(x => x.ActivityId).Take(4).ToList();
        }
        public async Task<List<UserWithActivityViewModel>> GetfeaturedActivitiesWithDistristAsync()
        {
            List<UserWithActivityViewModel> featuredActivities = await GetActivitiesWithDistristAsync();
            return featuredActivities.OrderByDescending(x => x.MemberQuantity).Take(4).ToList();
        }
    }
}
