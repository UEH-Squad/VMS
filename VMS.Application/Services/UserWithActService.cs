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

        public async Task<List<UserWithActivityViewModel>> GetNearestActivitiesAsync()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            List<Activity> activities = await _repository.GetListAsync<Activity>(context);

            IEnumerable<ActivityViewModel> Activities = activities.Select(x => new ActivityViewModel
            {
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,                
            }
            );

            //Seeding to user loacation
            var user = new UserViewModel();
            user.Lat = 14.155555;
            user.Long = 125.124444;

            List<ActivityViewModel> activitiesList = Activities.ToList();

            IEnumerable<UserWithActivityViewModel> userWithActivity = activitiesList.Select(x => new UserWithActivityViewModel
            {
                Name = x.Name,
                Distance = Haversine(user,x.Latitude,x.Longitude),
                MemberQuantity = x.MemberQuantity
            });

            List<UserWithActivityViewModel> ActivitiesList = userWithActivity.ToList();
            ActivitiesList.Sort((e1, e2) =>
            {
                return e1.Distance.CompareTo(e2.Distance);
            });
            ActivitiesList.Sort((e1, e2) =>
            {
                return e1.MemberQuantity.CompareTo(e2.MemberQuantity);
            });
            return ActivitiesList;
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
        }

        public async Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesAsync()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            List<Activity> activities = await _repository.GetListAsync<Activity>(context);

            IEnumerable<ActivityViewModel> Activities = activities.Select(x => new ActivityViewModel
            {
                Name = x.Name,                
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
                Name = x.Name,
                MemberQuantity = x.MemberQuantity
            });

            List<UserWithActivityViewModel> FeaturesActivitiesList = userWithActivity.ToList();
            FeaturesActivitiesList.Sort((e1, e2) =>
            {
                return e2.MemberQuantity.CompareTo(e1.MemberQuantity);
            });

            return FeaturesActivitiesList;
        }
    }
}
