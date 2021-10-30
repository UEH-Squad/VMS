using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
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

        public async Task<PaginatedList<ActivityViewModel>> GetAllActivitiesAsync(bool isSearch, string searchValue, FilterActivityViewModel filter, int currentPage, Dictionary<ActOrderBy, bool> orderList = null, Coordinate userLocation = null)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginatedList<ActivityViewModel> paginatedList;

            if (isSearch)
            {
                paginatedList = await GetAllActivitiesWithSearchValueAsync(searchValue, filter.OrgId, dbContext, currentPage, orderList, userLocation);
            }
            else
            {
                paginatedList = await GetAllActivitiesWithFilterAsync(filter, dbContext, currentPage, orderList, userLocation);
            }

            return paginatedList;
        }

        private async Task<PaginatedList<ActivityViewModel>> GetAllActivitiesWithSearchValueAsync(string searchValue, string orgId, DbContext dbContext, int currentPage, Dictionary<ActOrderBy, bool> orderList, Coordinate userLocation)
        {
            PaginationSpecification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    GetFilterByDate(),
                    (!string.IsNullOrEmpty(orgId) ? a => a.OrgId == orgId : a => true),
                    a => a.Name.ToUpper().Trim().Contains(searchValue.ToUpper().Trim()),
                    a => !a.IsDeleted
                },
                Includes = a => a.Include(x => x.Recruitments).ThenInclude(x => x.RecruitmentRatings),
                PageIndex = currentPage,
                PageSize = 20,
                OrderBy = GetOrderActivities(orderList, userLocation)
            };

            PaginatedList<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            activities.Items.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            var paginatedList = _mapper.Map<PaginatedList<ActivityViewModel>>(activities);

            paginatedList.Items.ForEach(a => a.Rating = GetRateOfActivity(activities.Items.FirstOrDefault(x => x.Id == a.Id).Recruitments));

            return paginatedList;
        }

        private async Task<PaginatedList<ActivityViewModel>> GetAllActivitiesWithFilterAsync(FilterActivityViewModel filter, DbContext dbContext, int currentPage, Dictionary<ActOrderBy, bool> orderList, Coordinate userLocation)
        {
            PaginationSpecification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    GetFilterByDate(filter.TookPlace, filter.Happenning),
                    a => a.IsVirtual == filter.Virtual || a.IsActual == filter.Actual || !filter.Virtual && !filter.Actual,
                    a => a.ActivityAddresses.Any(x => x.AddressPathId == filter.AddressPathId) || filter.AddressPathId == 0,
                    a => a.OrgId == filter.OrgId || string.IsNullOrEmpty(filter.OrgId),
                    a => filter.Areas.Select(x => x.Id).Any(z => z == a.AreaId) || filter.Areas.Count == 0,
                    a => a.ActivitySkills.Select(activitySkills => activitySkills.SkillId)
                                         .Where(actSkillId => filter.Skills.Select(skill => skill.Id)
                                                                           .Any(skillId => skillId == actSkillId))
                                         .Count() == filter.Skills.Count,
                    a => !a.IsDeleted
                },
                Includes = a => a.Include(x => x.Recruitments).ThenInclude(x => x.RecruitmentRatings),
                PageIndex = currentPage,
                PageSize = 20,
                OrderBy = GetOrderActivities(orderList, userLocation)
            };

            PaginatedList<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            activities.Items.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            var paginatedList = _mapper.Map<PaginatedList<ActivityViewModel>>(activities);

            paginatedList.Items.ForEach(a => a.Rating = GetRateOfActivity(activities.Items.FirstOrDefault(x => x.Id == a.Id).Recruitments));

            return paginatedList;
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
            activity.Location = _geometryFactory.CreatePoint(coordinateResponse);
            activity.Latitude = coordinateResponse.Y;
            activity.Longitude = coordinateResponse.X;
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
            activity.Location = _geometryFactory.CreatePoint(coordinateResponse);
            activity.Latitude = coordinateResponse.Y;
            activity.Longitude = coordinateResponse.X;

            activity.ActivitySkills = MapSkills(activityViewModel, activity);
            activity.ActivityAddresses = MapActivityAddresses(activityViewModel, activity);

            // Check if any update on Start Date (to extend Register time..v.v)
            if (activity.StartDate > DateTime.Now)
            {
                activity.IsClosed = false;
            }

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
                                                            .FirstOrDefault(x => x.AddressPath.Depth == 2)?.AddressPathId;
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
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    x => x.EndDate >= DateTime.Now
                },
                OrderBy = GetOrderByClause(isFeatured, location),
                Take = 4
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);

            return activities;
        }

        private async Task<List<Activity>> GetRelatedActivitiesForUsersTurnOffLocationAsync(bool isFeatured, int? currentUserDistricts, DbContext dbContext)
        {
            Specification<Activity> spec = new()
            {
                Includes = x => x.Include(y => y.ActivityAddresses).ThenInclude(y => y.AddressPath),
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    x => x.ActivityAddresses.Any(y => y.AddressPath.Id == currentUserDistricts),
                    x => x.EndDate >= DateTime.Now
                },
                OrderBy = GetOrderByClause(isFeatured, null),
                Take = 4
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);
            return activities;
        }

        private async Task<List<Activity>> GetRelatedActivitiesForNonUsersTurnOffLocationAsync(bool isFeatured, DbContext dbContext)
        {
            Specification<Activity> spec = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>
                {
                    x => x.EndDate >= DateTime.Now
                },
                OrderBy = GetOrderByClause(isFeatured, null),
                Take = 4
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, spec);
            return activities;
        }

        private Func<IQueryable<Activity>, IOrderedQueryable<Activity>> GetOrderByClause(bool isFeatured, Coordinate coordinate)
        {
            Point userLocation = _geometryFactory.CreatePoint(coordinate);
            Func<IQueryable<Activity>, IOrderedQueryable<Activity>> result = isFeatured
                ? (x => x.OrderByDescending(y => y.MemberQuantity))
                : (x => x.OrderByDescending(y => y.CreatedDate));

            if (coordinate is not null)
            {
                result = isFeatured
                    ? (x => x.OrderBy(y => y.Location.Distance(userLocation))
                             .ThenByDescending(y => y.MemberQuantity))
                    : (x => x.OrderBy(y => y.Location.Distance(userLocation))
                             .ThenByDescending(y => y.CreatedDate));
            }

            return result;
        }

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

        public async Task<List<ActivityViewModel>> GetOrgActs(string id, StatusAct status)
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.OrgId == id,
                    status == StatusAct.Favor? a => a.Favorites.Count >=0 : (status != StatusAct.Ended ? a=> a.EndDate >= DateTime.Now : a => a.EndDate < DateTime.Now),
                    a => a.StartDate <= DateTime.Now,
                    a => a.IsDeleted == false
                },
                Includes = activities => activities.Include(x => x.Recruitments).ThenInclude(x => x.RecruitmentRatings)
                                                    .Include(x => x.Favorites)
                                                    .Include(x=>x.ActivityAddresses).ThenInclude(x=> x.AddressPath)
            };

            List<Activity> activity = await _repository.GetListAsync<Activity>(context, specification);
            string currentId = _identityService.GetCurrentUserId();
            IEnumerable<ActivityViewModel> activityViewModels = activity.Select(x => new ActivityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Banner = x.Banner,
                IsClosed = x.IsClosed,
                Favorites = x.Favorites.Count,
                MemberQuantity = x.MemberQuantity,
                Province = GetActProvince(x.ActivityAddresses),
                EndDate = x.EndDate,
                Rating = GetRateOfActivity(x.Recruitments),
                IsFav = GetActFavor(x.Id,currentId,x.Favorites)
            });
            return status switch
            {
               StatusAct.Current => activityViewModels.ToList(),
               StatusAct.Favor => activityViewModels.OrderByDescending(a => a.Favorites).Take(8).ToList(),
               StatusAct.Ended => activityViewModels.OrderByDescending(a => a.EndDate).Take(4).ToList(),
                _ => null
            };
        }

        private static bool GetActFavor(int actId, string userId, ICollection<Favorite> favorites)
        {
            Favorite favorite = favorites.Where(a => a.ActivityId == actId && a.UserId == userId).FirstOrDefault();
            if (favorite != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string GetActProvince(ICollection<ActivityAddress> activityAddresses)
        {
            ActivityAddress address = activityAddresses.Where(a => a.AddressPath.Depth == 1).FirstOrDefault();
            if (address != null)
            {
                return address.AddressPath.Name;
            }
            else
            {
                return "Hồ Chí Minh"; 
            }
        }

        private static double GetRateOfActivity(ICollection<Recruitment> recruitments)
        {
            return recruitments.Sum(a => a.RecruitmentRatings.Where(x => !x.IsOrgRating && !x.IsReport).Sum(x => x.Rank))
                    / recruitments.Sum(a => a.RecruitmentRatings.Where(x => !x.IsOrgRating && !x.IsReport).Count());
        }

        private Func<IQueryable<Activity>, IOrderedQueryable<Activity>> GetOrderActivities(Dictionary<ActOrderBy, bool> orderList, Coordinate coordinate)
        {
            Point userLocation = _geometryFactory.CreatePoint(coordinate);

            if (orderList == null || userLocation == null)
            {
                return x => x.OrderByDescending(a => a.Id);
            }

            if (orderList[ActOrderBy.Newest] && orderList[ActOrderBy.Nearest] && orderList[ActOrderBy.Hottest])
            {
                return x => x.OrderByDescending(a => a.Id)
                            .ThenByDescending(a => a.MemberQuantity)
                            .ThenBy(a => a.Location.Distance(userLocation));
            }

            if (orderList[ActOrderBy.Newest] && orderList[ActOrderBy.Hottest])
            {
                return x => x.OrderByDescending(a => a.Id)
                            .ThenByDescending(a => a.MemberQuantity);
            }

            if (orderList[ActOrderBy.Newest] && orderList[ActOrderBy.Nearest])
            {
                return x => x.OrderByDescending(a => a.Id)
                            .ThenBy(a => a.Location.Distance(userLocation));
            }

            if (orderList[ActOrderBy.Hottest] && orderList[ActOrderBy.Nearest])
            {
                return x => x.OrderByDescending(a => a.MemberQuantity)
                            .ThenBy(a => a.Location.Distance(userLocation));
            }

            if (orderList[ActOrderBy.Hottest])
            {
                return x => x.OrderByDescending(a => a.MemberQuantity);
            }

            if (orderList[ActOrderBy.Nearest])
            {
                return x => x.OrderBy(a => a.Location.Distance(userLocation));
            }

            return x => x.OrderByDescending(a => a.Id);
        }

        private static Expression<Func<Activity, bool>> GetFilterByDate(bool isTookPlace = false, bool isHappenning = false)
        {
            if (isTookPlace && isHappenning)
            {
                return x => x.EndDate < DateTime.Now || x.EndDate >= DateTime.Now;
            }

            if (isTookPlace)
            {
                return x => x.EndDate < DateTime.Now;
            }

            if (isHappenning)
            {
                return x => x.EndDate >= DateTime.Now;
            }

            return x => x.EndDate < DateTime.Now || x.EndDate >= DateTime.Now;
        }

        public async Task CloseOrDeleteActivity(int activityId, bool isClose = false, bool isDelete = false)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    a => a.Id == activityId
                }
            };

            Activity activity = await _repository.GetAsync(dbContext, specification);

            activity.IsClosed = isClose;
            activity.IsDeleted = isDelete;

            await _repository.UpdateAsync(dbContext, activity);
        }

        public async Task UpdateActFavorAsync(int activityId, string userId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Favorite> specification = new()
            {
                Conditions = new List<Expression<Func<Favorite, bool>>>
                {
                    a => a.ActivityId == activityId,
                    a => a.UserId == userId
                }
            };

            Favorite favorite = await _repository.GetAsync(dbContext, specification);

            if(favorite ==null)
            {
                Favorite fav = new();
                fav.ActivityId = activityId;
                fav.UserId = userId;
                fav.CreatedDate = DateTime.Now;
                await _repository.InsertAsync(dbContext, fav);
            }
            else
            {
                await _repository.DeleteAsync(dbContext, favorite);
            }
        }

        public async Task CloseActivityDailyAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<Expression<Func<Activity, bool>>>()
                {
                    a => a.StartDate <= DateTime.Now,
                    a => !a.IsClosed
                }
            };

            List<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            activities.ForEach(a => a.IsClosed = true);

            await _repository.UpdateAsync<Activity>(dbContext, activities);
        }
    }
}