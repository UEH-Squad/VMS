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
        private readonly IAddressLocationApiService _addressLocationApiService;

        public ActivityService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper, IIdentityService identityService, IAddressLocationApiService addressLocationApiService) : base(repository, dbContextFactory, mapper)
        {
            _identityService = identityService;
            _addressLocationApiService = addressLocationApiService;
        }

        public async Task<List<ActivityViewModel>> GetAllActivitiesAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Includes = a => a.Include(x => x.ActivitySkills)
            };
            List<Activity> activities = await _repository.GetListAsync<Activity>(dbContext);
            activities.ForEach(a => a.Organizer = _identityService.FindUserById(a.OrgId));

            List<ActivityViewModel> activitiesViewModel = _mapper.Map<List<ActivityViewModel>>(activities);
            return activitiesViewModel;
        }

        public async Task AddActivityAsync(CreateActivityViewModel activityViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Activity activity = _mapper.Map<Activity>(activityViewModel);
            activity.PostDate = DateTime.Now;
            activity.IsApproved = false;
            activity.CreatedBy = activity.OrgId;
            activity.CreatedDate = DateTime.Now;

            AddressLocationReponse addressLocationReponse = await _addressLocationApiService.GetAddressLocationAsync(activity.Address);
            activity.Latitude = addressLocationReponse.Latitude;
            activity.Longitude = addressLocationReponse.Longitude;

            await _repository.InsertAsync(dbContext, activity);

            List<ActivitySkill> activitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                ActivityId = activity.Id,
                SkillId = s.Id,
                IsDeleted = false
            }).ToList();
            await _repository.InsertAsync<ActivitySkill>(dbContext, activitySkills);
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
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
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
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
            };
            Activity activity = await _repository.GetAsync(dbContext, specification);

            activity = _mapper.Map(activityViewModel, activity);

            activity.ActivitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                SkillId = s.Id,
                ActivityId = activity.Id,
                IsDeleted = false
            }).ToList() ;

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
