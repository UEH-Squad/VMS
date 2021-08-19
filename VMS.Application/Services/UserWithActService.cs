using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IIdentityService _identityService;

        public UserWithActService(IIdentityService identityService,
                                  IRepository repository,
                                  IDbContextFactory<VmsDbContext> dbContextFactory,
                                  IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
            _identityService = identityService;
        }

        public async Task<List<UserWithActivityViewModel>> GetRelatedActivities(string userId, UserLocation location, bool isFeatured = false)
        {
            await using DbContext dbContext = _dbContextFactory.CreateDbContext();
            List<Activity> activities;

            if (string.IsNullOrEmpty(userId) && location == null)
            {
                activities = await GetRelatedActivitiesForNonUsersTurnOffLocationAsync(isFeatured, dbContext);
                return _mapper.Map<List<UserWithActivityViewModel>>(activities);
            }

            if (!string.IsNullOrEmpty(userId) && location == null)
            {
                int? currentUserDistricts = _identityService.GetCurrentUserWithAddresses()?.UserAddresses
                    .FirstOrDefault(x => x.AddressPath.Depth == 2)
                    ?.AddressPathId;
                if (currentUserDistricts.HasValue)
                {
                    activities = await GetRelatedActivitiesForUsersTurnOffLocationAsync(isFeatured, currentUserDistricts, dbContext);
                    return _mapper.Map<List<UserWithActivityViewModel>>(activities);
                }

                activities = await GetRelatedActivitiesForNonUsersTurnOffLocationAsync(isFeatured, dbContext);
                return _mapper.Map<List<UserWithActivityViewModel>>(activities);
            }

            activities = await GetRelatedActivitiesWhenLocationTurnedOnAsync(location, isFeatured, dbContext);
            return _mapper.Map<List<UserWithActivityViewModel>>(activities);
        }

        private async Task<List<Activity>> GetRelatedActivitiesWhenLocationTurnedOnAsync(UserLocation location, bool isFeatured, DbContext dbContext)
        {
            Specification<Activity> spec = new()
            {
                OrderBy = GetOrderByClause(isFeatured)
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);
            activities = activities.OrderByDescending(x => Haversine(location.Lat, location.Long, x.Latitude, x.Longitude))
                                   .Take(4)
                                   .ToList();
            return activities;
        }

        private async Task<List<Activity>> GetRelatedActivitiesForUsersTurnOffLocationAsync(bool isFeatured, int? currentUserDistricts, DbContext dbContext)
        {
            Specification<Activity> spec = new()
            {
                Includes = x => x.Include(y => y.ActivityAddresses).ThenInclude(y => y.AddressPath),
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    x => x.ActivityAddresses.Any(y => y.AddressPath.Id == currentUserDistricts)
                },
                OrderBy = GetOrderByClause(isFeatured),
                Take = 4
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);
            return activities;
        }

        private async Task<List<Activity>> GetRelatedActivitiesForNonUsersTurnOffLocationAsync(bool isFeatured, DbContext dbContext)
        {
            Specification<Activity> spec = new()
            {
                OrderBy = GetOrderByClause(isFeatured),
                Take = 4
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);
            return activities;
        }

        private static Func<IQueryable<Activity>, IOrderedQueryable<Activity>> GetOrderByClause(bool isFeatured)
            => isFeatured ? (x => x.OrderByDescending(y => y.MemberQuantity)) : (x => x.OrderByDescending(y => y.Id));

        private static double Haversine(double lat1, double long1, double lat2, double long2)
        {
            /* Source: https://www.geeksforgeeks.org/haversine-formula-to-find-distance-between-two-points-on-a-sphere/ */

            double latDistance = (Math.PI / 180) * (lat2 - lat1);
            double longDistance = (Math.PI / 180) * (long2 - long1);
            double userLatRadian = (Math.PI / 180) * lat1;
            double actLatRadian = (Math.PI / 180) * lat2;
            double formula = Math.Pow(Math.Sin(latDistance / 2), 2) + Math.Pow(Math.Sin(longDistance / 2), 2) * Math.Cos(userLatRadian) * Math.Cos(actLatRadian);
            double distance = Math.Round(2 * 6371 * Math.Asin(Math.Sqrt(formula)), 2);
            return distance;
        }
    }
}