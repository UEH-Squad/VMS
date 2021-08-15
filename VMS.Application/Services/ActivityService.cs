using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAddressLocationService _addressLocationService;

        public ActivityService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper, IIdentityService identityService, IAddressLocationService addressLocationService) : base(repository, dbContextFactory, mapper)
        {
            _identityService = identityService;
            _addressLocationService = addressLocationService;
        }


        public async Task<List<ActivityViewModel>> GetAllActivitiesAsync(FilterActivityViewModel filter)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Includes = a => a.Include(a => a.ActivitySkills),
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>()
                {
                    a => a.IsVirtual == filter.Virtual || a.IsVirtual == filter.Actual
                }
            };

            if (!string.IsNullOrEmpty(filter.OrgId))
            {
                specification.Conditions.Add(a => a.OrgId == filter.OrgId);
            }

            if (filter.ProvinceId != 0)
            {
                specification.Conditions.Add(a => a.ActivityAddresses.FirstOrDefault(x => x.AddressPathId == filter.ProvinceId) != null);

                if (filter.DistrictId != 0)
                {
                    specification.Conditions.Add(a => a.ActivityAddresses.FirstOrDefault(x => x.AddressPathId == filter.DistrictId) != null);
                }
            }

            List<Activity> activities = await _repository.GetListAsync(dbContext, specification);

            if (filter.Areas.Count != 0)
            {
                activities = activities.Where(a => filter.Areas.Any(r => r.Id == a.AreaId)).ToList();
            }

            if (filter.Skills.Count != 0)
            {
                activities = activities.Where(a => filter.Skills
                                                .All(s => a.ActivitySkills
                                                .Any(x => x.SkillId == s.Id)))
                                                .ToList();
            }

            activities.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            List<ActivityViewModel> activitiesViewModel = _mapper.Map<List<ActivityViewModel>>(activities);

            return activitiesViewModel.OrderByDescending(a => a.PostDate).ToList();
        }

        public async Task AddActivityAsync(CreateActivityViewModel activityViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Activity activity = _mapper.Map<Activity>(activityViewModel);
            activity.PostDate = DateTime.Now;
            activity.IsApproved = false;
            activity.CreatedBy = activity.OrgId;
            activity.CreatedDate = DateTime.Now;

            CoordinateResponse coordinateResponse = await _addressLocationService.GetCoordinateAsync(activityViewModel.FullAddress);
            activity.Latitude = coordinateResponse.Lat;
            activity.Longitude = coordinateResponse.Long;

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
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
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
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
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

            CoordinateResponse coordinateResponse = await _addressLocationService.GetCoordinateAsync(activityViewModel.FullAddress);
            activity.Latitude = coordinateResponse.Lat;
            activity.Longitude = coordinateResponse.Long;

            activity.ActivitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                SkillId = s.Id,
                ActivityId = activity.Id,
                IsDeleted = false
            }).ToList() ;

            activity.ActivityAddresses = new List<ActivityAddress>()
            {
                new() { Activity = activity, AddressPathId = activityViewModel.ProvinceId },
                new() { Activity = activity, AddressPathId = activityViewModel.DistrictId },
                new() { Activity = activity, AddressPathId = activityViewModel.WardId }
            }; 

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
    }
}
