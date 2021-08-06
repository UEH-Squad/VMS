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

        public async Task<List<UserWithActivityViewModel>> GetAllActivities()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            List<Activity> activities = await _repository.GetListAsync<Activity>(context);

            IEnumerable<ActivityViewModel> Activities = activities.Select(x => new ActivityViewModel
            {
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }
            );

            //Seeding to user loacation
            var user = new UserViewModel();
            user.Latitude = 14.155555;
            user.Longitude = 125.124444;

            List<ActivityViewModel> acts = Activities.ToList();

            IEnumerable<UserWithActivityViewModel> userWithAct = acts.Select(x => new UserWithActivityViewModel
            {
                Name = x.Name,
                Distance = Haversine(user,x.Latitude,x.Longitude)
            });

            List<UserWithActivityViewModel> UserWithActList = userWithAct.ToList();
            UserWithActList.Sort((e1, e2) =>
            {
                return e1.Distance.CompareTo(e2.Distance);
            });

            return UserWithActList;
        }
        static double Haversine(UserViewModel user, double latitude, double longitude)
        {
            double dLat = (Math.PI / 180) * (latitude - user.Latitude);
            double dLon = (Math.PI / 180) * (longitude - user.Longitude);

            double rLat1 = (Math.PI / 180) * user.Latitude;
            double rLat2 = (Math.PI / 180) * latitude;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(rLat1) * Math.Cos(rLat2);

            double d = Math.Round(2 * 6371 * Math.Asin(Math.Sqrt(a)), 2);

            return d;
        }
    }
}
