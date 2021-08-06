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
                Lat = x.Latitude,
                Lon = x.Longitude
            }
            );

            //Seeding to user loacation
            UserViewModel user = new UserViewModel();
            user.Lat = 14.155555;
            user.Lon = 125.124444;

            List<ActivityViewModel> acts = Activities.ToList();

            List<UserWithActivityViewModel> userWithAct = new List<UserWithActivityViewModel>();

            if (acts != null)
            {
                for (int i = 0; i < acts.Count(); i++)
                {
                    userWithAct.Add(new UserWithActivityViewModel());
                    userWithAct[i].Name = acts[i].Name;
                    userWithAct[i].Distance = Haversine(user, acts[i]);
                }
            }
            else
            {
                return null;
            }
            userWithAct.Sort((e1, e2) =>
            {
                return e1.Distance.CompareTo(e2.Distance);
            });

            return userWithAct;
        }
        static double Haversine(UserViewModel user, ActivityViewModel acts)
        {
            double dLat = (Math.PI / 180) * (acts.Lat - user.Lat);
            double dLon = (Math.PI / 180) * (acts.Lon - user.Lon);

            double rLat1 = (Math.PI / 180) * user.Lat;
            double rLat2 = (Math.PI / 180) * acts.Lat;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(rLat1) * Math.Cos(rLat2);

            double d = Math.Round(2 * 6371 * Math.Asin(Math.Sqrt(a)), 2);

            return d;
        }
    }
}
