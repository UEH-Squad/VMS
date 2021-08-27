using AutoMapper;
using Geolocation;
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
using Coordinate = VMS.Application.ViewModels.Coordinate;

namespace VMS.Application.Services
{
    public class ActivityService : BaseService, IActivityService
    {
        private readonly IIdentityService _identityService;
        private readonly IGeoLocationService _addressLocationService;

        public ActivityService(IRepository repository,
                               IDbContextFactory<VmsDbContext> dbContextFactory,
                               IMapper mapper,
                               IIdentityService identityService,
                               IGeoLocationService addressLocationService) : base(repository, dbContextFactory, mapper)
        {
            _identityService = identityService;
            _addressLocationService = addressLocationService;
        }

        public async Task<List<ActivityViewModel>> GetAllActivitiesAsync(bool isSearch, string searchValue, FilterActivityViewModel filter)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            if (isSearch)
            {
                return await GetAllActivitiesWithSearchValueAsync(searchValue, dbContext);
            }
            else
            {
                return await GetAllActivitiesWithFilterAsync(filter, dbContext);
            }
        }

        private async Task<List<ActivityViewModel>> GetAllActivitiesWithSearchValueAsync(string searchValue, DbContext dbContext)
        {
            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    a => a.Name.ToUpper().Trim().Contains(searchValue.ToUpper().Trim())
                }
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            activities.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            return _mapper.Map<List<ActivityViewModel>>(activities);
        }

        private async Task<List<ActivityViewModel>> GetAllActivitiesWithFilterAsync(FilterActivityViewModel filter, DbContext dbContext)
        {
            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    a => a.IsVirtual == filter.Virtual || a.IsVirtual == filter.Actual,
                    a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.AddressPathId) || filter.AddressPathId == 0,
                    a => a.OrgId == filter.OrgId || string.IsNullOrEmpty(filter.OrgId),
                    a => filter.Areas.Any(x => x == a.AreaId) || filter.Areas.Count == 0,
                    //a => filter.Skills.All(s => a.ActivitySkills.Any(x => x.SkillId == s.Id))
                },
                Includes = a => a.Include(x => x.ActivitySkills)
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            activities = activities.Where(a => filter.Skills.All(s => a.ActivitySkills.Any(x => x.SkillId == s.Id))).ToList();

            activities.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            return _mapper.Map<List<ActivityViewModel>>(activities);
        }

        public async Task<List<ActivityViewModel>> GetFeaturedActivitiesAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    a => a.IsPin
                },
                Take = 2
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<List<ActivityViewModel>>(activities);
        }

        public async Task AddActivityAsync(CreateActivityViewModel activityViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Activity activity = _mapper.Map<Activity>(activityViewModel);
            activity.PostDate = DateTime.Now;
            activity.IsApproved = false;
            activity.CreatedBy = activity.OrgId;
            activity.CreatedDate = DateTime.Now;

            Coordinate coordinateResponse = await _addressLocationService.GetCoordinateAsync(activityViewModel.FullAddress);
            activity.Latitude = coordinateResponse.Latitude;
            activity.Longitude = coordinateResponse.Longitude;

            activity.ActivitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                Activity = activity,
                SkillId = s.Id,
                IsDeleted = false
            }).ToList();

            activity.ActivityAddresses = new List<ActivityAddress>()
            {
                new() { Activity = activity, AddressPathId = activityViewModel.ProvinceId },
                new() { Activity = activity, AddressPathId = activityViewModel.DistrictId },
                new() { Activity = activity, AddressPathId = activityViewModel.WardId }
            };

            await _repository.InsertAsync(dbContext, activity);
        }

        public async Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    a => a.Id == activityId
                },
                Includes = a => a.Include(x => x.ActivitySkills)
                                    .ThenInclude(s => s.Skill)
                                    .Include(x => x.ActivityAddresses)
                                    .ThenInclude(x => x.AddressPath)
            };
            Activity activity = await _repository.GetAsync(dbContext, specification);

            if (activity is null) return new CreateActivityViewModel();

            CreateActivityViewModel activityViewModel = _mapper.Map<CreateActivityViewModel>(activity);

            activityViewModel.Skills = activity.ActivitySkills.Select(a => new Skill
            {
                Id = a.SkillId,
                Name = a.Skill.Name,
                IsDeleted = a.Skill.IsDeleted
            }).ToList();

            List<ActivityAddress> activityAddresses = activity.ActivityAddresses.OrderBy(a => a.AddressPath.Depth).ToList();
            activityViewModel.ProvinceId = activityAddresses[0].AddressPathId;
            activityViewModel.DistrictId = activityAddresses[1].AddressPathId;
            activityViewModel.WardId = activityAddresses[2].AddressPathId;

            return activityViewModel;
        }

        public async Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    a => a.Id == activityId
                },
                Includes = a => a.Include(x => x.ActivitySkills)
                                    .ThenInclude(s => s.Skill)
                                    .Include(x => x.ActivityAddresses)
                                    .ThenInclude(s => s.AddressPath)
            };
            Activity activity = await _repository.GetAsync(dbContext, specification);

            activity = _mapper.Map(activityViewModel, activity);

            Coordinate coordinateResponse = await _addressLocationService.GetCoordinateAsync(activityViewModel.FullAddress);
            activity.Latitude = coordinateResponse.Latitude;
            activity.Longitude = coordinateResponse.Longitude;

            activity.ActivitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                SkillId = s.Id,
                ActivityId = activity.Id,
                IsDeleted = false
            }).ToList();

            activity.ActivityAddresses = new List<ActivityAddress>()
            {
                new() { Activity = activity, AddressPathId = activityViewModel.ProvinceId },
                new() { Activity = activity, AddressPathId = activityViewModel.DistrictId },
                new() { Activity = activity, AddressPathId = activityViewModel.WardId }
            };

            activity.UpdatedBy = activityViewModel.OrgId;
            activity.UpdatedDate = DateTime.Now;

            await _repository.UpdateAsync(dbContext, activity);
        }

        public async Task DeleteActivityAsync(int activityId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            Activity activity = await _repository.GetByIdAsync<Activity>(dbContext, activityId);
            await _repository.DeleteAsync(dbContext, activity);
        }

        public async Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.Id == activityId
                },
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
                                .Include(x => x.Area)
            };

            Activity activity = await _repository.GetAsync(dbContext, specification);

            if (activity is null) return new ViewActivityViewModel();

            ViewActivityViewModel activityViewModel = _mapper.Map<ViewActivityViewModel>(activity);

            activityViewModel.Skills = activity.ActivitySkills.Select(a => new Skill
            {
                Id = a.SkillId,
                Name = a.Skill.Name,
                IsDeleted = a.Skill.IsDeleted
            }).ToList();

            return activityViewModel;
        }

        public async Task<List<UserWithActivityViewModel>> GetRelatedActivities(string userId, Coordinate location, bool isFeatured = false)
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

        private async Task<List<Activity>> GetRelatedActivitiesWhenLocationTurnedOnAsync(Coordinate location, bool isFeatured, DbContext dbContext)
        {
            Specification<Activity> spec = new()
            {
                OrderBy = GetOrderByClause(isFeatured)
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);
            activities = activities.OrderByDescending(x => GeoCalculator.GetDistance(location.Latitude, location.Longitude, x.Latitude, x.Longitude, 2, DistanceUnit.Meters))
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
    }
}