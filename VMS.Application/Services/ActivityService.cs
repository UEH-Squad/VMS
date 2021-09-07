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

        public async Task<PagedResult<ActivityViewModel>> GetAllActivitiesAsync(bool isSearch, string searchValue, FilterActivityViewModel filter, Dictionary<ActOrderBy, bool> orderList, Coordinate userLocation, int currentPage)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            List<ActivityViewModel> activities;

            if (isSearch)
            {
                activities = await GetAllActivitiesWithSearchValueAsync(searchValue, dbContext);
            }
            else
            {
                activities = await GetAllActivitiesWithFilterAsync(filter, dbContext);
            }

            activities = GetOrderActivities(orderList, activities, userLocation);

            PagedResult<ActivityViewModel> result = new PagedResult<ActivityViewModel>
            {
                CurrentPage = currentPage,
                RowCount = activities.Count,
                PageSize = 20
            };
            result.PageCount = (int)Math.Ceiling(result.RowCount * 1.0 / result.PageSize);
            result.Results = activities.Skip((result.CurrentPage - 1) * result.PageSize).Take(result.PageSize).ToList();

            return result;
        }

        private async Task<List<ActivityViewModel>> GetAllActivitiesWithSearchValueAsync(string searchValue, DbContext dbContext)
        {
            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    a => a.Name.ToUpper().Trim().Contains(searchValue.ToUpper().Trim()),
                    a => a.EndDate >= DateTime.Now
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
                    a => a.IsVirtual == filter.Virtual || a.IsVirtual == !filter.Actual,
                    a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.AddressPathId) || filter.AddressPathId == 0,
                    a => a.OrgId == filter.OrgId || string.IsNullOrEmpty(filter.OrgId),
                    a => filter.Areas.Select(x => x.Id).Any(z => z == a.AreaId) || filter.Areas.Count == 0,
                    a => a.ActivitySkills.Select(activitySkills => activitySkills.SkillId)
                                         .Where(actSkillId => filter.Skills.Select(skill => skill.Id)
                                                                           .Any(skillId => skillId == actSkillId))
                                         .Count() == filter.Skills.Count,
                    a => a.EndDate >= DateTime.Now
                },
                Includes = a => a.Include(x => x.ActivitySkills)
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            activities.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            return _mapper.Map<List<ActivityViewModel>>(activities);
        }

        private static List<ActivityViewModel> GetOrderActivities(Dictionary<ActOrderBy, bool> orderList, List<ActivityViewModel> activities, Coordinate userLocation)
        {
            if (orderList[ActOrderBy.Newest] && orderList[ActOrderBy.Nearest] && orderList[ActOrderBy.Hottest])
            {
                return activities = activities.OrderByDescending(a => a.PostDate)
                                            .ThenByDescending(a => a.MemberQuantity)
                                            .ThenBy(a => GeoCalculator.GetDistance(userLocation.Latitude, userLocation.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters))
                                            .ToList();
            }

            if (orderList[ActOrderBy.Newest] && orderList[ActOrderBy.Hottest])
            {
                return activities = activities.OrderByDescending(a => a.PostDate)
                                            .ThenByDescending(a => a.MemberQuantity)
                                            .ToList();
            }

            if (orderList[ActOrderBy.Newest] && orderList[ActOrderBy.Nearest])
            {
                return activities = activities.OrderByDescending(a => a.PostDate)
                                            .ThenBy(a => GeoCalculator.GetDistance(userLocation.Latitude, userLocation.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters))
                                            .ToList();
            }

            if (orderList[ActOrderBy.Hottest] && orderList[ActOrderBy.Nearest])
            {
                return activities = activities.OrderByDescending(a => a.MemberQuantity)
                                            .ThenBy(a => GeoCalculator.GetDistance(userLocation.Latitude, userLocation.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters))
                                            .ToList();
            }

            if (orderList[ActOrderBy.Hottest])
            {
                return activities = activities.OrderByDescending(a => a.MemberQuantity).ToList();
            }

            if (orderList[ActOrderBy.Nearest])
            {
                return activities = activities.OrderBy(a => GeoCalculator.GetDistance(userLocation.Latitude, userLocation.Longitude, a.Coordinate.Latitude, a.Coordinate.Longitude, 2, DistanceUnit.Meters)).ToList();
            }

            return activities = activities.OrderByDescending(a => a.PostDate).ToList();
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

        public async Task<int> AddActivityAsync(CreateActivityViewModel activityViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Activity activity = _mapper.Map<Activity>(activityViewModel);
            activity.CreatedDate = DateTime.Now;
            activity.CreatedBy = activity.OrgId;
            activity.IsApproved = true;

            Coordinate coordinateResponse = await _addressLocationService.GetCoordinateAsync(activityViewModel.FullAddress);
            activity.Latitude = coordinateResponse.Latitude;
            activity.Longitude = coordinateResponse.Longitude;
            activity.ActivitySkills = MapSkills(activityViewModel, activity);
            activity.ActivityAddresses = MapActivityAddresses(activityViewModel, activity);

            object[] id = await _repository.InsertAsync(dbContext, activity);

            return Convert.ToInt32(id[0]);
        }

        public async Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Activity activity = await _repository.GetAsync(dbContext, new Specification<Activity>()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    a => a.Id == activityId
                },
                Includes = a => a.Include(x => x.ActivitySkills)
                                    .ThenInclude(s => s.Skill)
                                 .Include(x => x.ActivityAddresses)
                                    .ThenInclude(x => x.AddressPath)
                                 .Include(x => x.Area)
            });

            if (activity is null)
            {
                return null;
            }

            CreateActivityViewModel activityViewModel = _mapper.Map<CreateActivityViewModel>(activity);

            activityViewModel.AreaName = activity.Area.Name;
            activityViewModel.AreaIcon = activity.Area.Icon;
            activityViewModel.Skills = activity.ActivitySkills.Select(a => new SkillViewModel
            {
                Id = a.SkillId,
                Name = a.Skill.Name
            }).ToList();

            ActivityAddress province = activity.ActivityAddresses.FirstOrDefault(x => x.AddressPath.Depth == 1);
            if (province is not null)
            {
                activityViewModel.ProvinceId = province.AddressPath.Id;
                activityViewModel.Province = province.AddressPath.Name;
            }

            ActivityAddress district = activity.ActivityAddresses.FirstOrDefault(x => x.AddressPath.Depth == 2);
            if (district is not null)
            {
                activityViewModel.DistrictId = district.AddressPath.Id;
                activityViewModel.District = district.AddressPath.Name;
            }

            ActivityAddress ward = activity.ActivityAddresses.FirstOrDefault(x => x.AddressPath.Depth == 3);
            if (ward is not null)
            {
                activityViewModel.WardId = ward.AddressPath.Id;
                activityViewModel.Ward = ward.AddressPath.Name;
            }

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
            activity.UpdatedBy = activity.OrgId;
            activity.UpdatedDate = DateTime.Now;

            Coordinate coordinateResponse = await _addressLocationService.GetCoordinateAsync(activityViewModel.FullAddress);
            activity.Latitude = coordinateResponse.Latitude;
            activity.Longitude = coordinateResponse.Longitude;

            activity.ActivitySkills = MapSkills(activityViewModel, activity);
            activity.ActivityAddresses = MapActivityAddresses(activityViewModel, activity);

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

        private static ICollection<ActivitySkill> MapSkills(CreateActivityViewModel activityViewModel, Activity activity)
        {
            return activityViewModel.Skills.Select(s => new ActivitySkill
            {
                Activity = activity,
                SkillId = s.Id
            }).ToList();
        }

        private static ICollection<ActivityAddress> MapActivityAddresses(CreateActivityViewModel activityViewModel, Activity activity)
        {
            List<ActivityAddress> result = new()
            {
                new() { Activity = activity, AddressPathId = activityViewModel.ProvinceId },
                new() { Activity = activity, AddressPathId = activityViewModel.DistrictId }
            };

            if (activityViewModel.WardId > 0)
            {
                result.Add(new() { Activity = activity, AddressPathId = activityViewModel.WardId });
            }

            return result;
        }
    }
}