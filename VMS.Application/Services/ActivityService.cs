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
        public ActivityService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }
        public async Task<List<ActivityViewModel>> GetAllActivities()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            // get all activities from database
            List<Activity> activities = await _repository.GetListAsync<Activity>(dbContext);

            // transform Activity model to ActivityViewModel
            IEnumerable<ActivityViewModel> activitiesViewModel = activities.Select(a => new ActivityViewModel
            {
                Id = a.Id,
                Name = a.Name,
                StartDate = a.StartDate,
                EndDay = a.EndDate,
                MemberQuantity = a.MemberQuantity,
                Description = a.Description,
                Mission = a.Mission,
                Banner = a.Banner
            }).OrderByDescending(a => a.Id);
            return activitiesViewModel.ToList();
        }

        public async Task AddActivity(CreateActivityViewModel activityViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Activity activity = new Activity
            {
                OrgId = activityViewModel.OrgId,
                AreaId = activityViewModel.AreaId,
                Name = activityViewModel.Name,
                StartDate = activityViewModel.StartDate,
                Address = activityViewModel.Address,
                EndDate = activityViewModel.EndDate,
                MemberQuantity = activityViewModel.MemberQuantity,
                Description = activityViewModel.Description,
                Mission = activityViewModel.Mission,
                IsVirtual = activityViewModel.IsVirtual,
                Website = activityViewModel.Website,
                Banner = activityViewModel.Banner,
                IsApproved = false,

                CreatedBy = activityViewModel.OrgId,
                CreatedDate = DateTime.Now
            };
            await _repository.InsertAsync(dbContext, activity);

            List<ActivitySkill> activitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                ActivityId = activity.Id,
                SkillId = s.Id,
                IsDeleted = false
            }).ToList();
            await _repository.InsertAsync<ActivitySkill>(dbContext, activitySkills);

            List<ActivityRequirement> activityRequirements = activityViewModel.Requirements.Select(r => new ActivityRequirement
            {
                ActivityId = activity.Id,
                RequirementId = r.Id,
                IsDeleted = false
            }).ToList();
            await _repository.InsertAsync<ActivityRequirement>(dbContext, activityRequirements);
        }

        public async Task<CreateActivityViewModel> GetCreateActivityViewModel(int id)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.Id == id
                },
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
                                .Include(x => x.ActivityRequirements).ThenInclude(r => r.Requirement)
            };

            Activity activity = await _repository.GetAsync(dbContext, specification);

            if (activity is null) return new CreateActivityViewModel();

            CreateActivityViewModel activityViewModel = new CreateActivityViewModel
            {
                OrgId = activity.OrgId,
                AreaId = activity.AreaId,
                Name = activity.Name,
                StartDate = activity.StartDate,
                Address = activity.Address,
                EndDate = activity.EndDate,
                MemberQuantity = activity.MemberQuantity,
                Description = activity.Description,
                Mission = activity.Mission,
                IsVirtual = activity.IsVirtual,
                Website = activity.Website,
                Banner = activity.Banner
            };

            activityViewModel.Skills = activity.ActivitySkills.Select(a => new Skill
            {
                Id = a.SkillId,
                Name = a.Skill.Name,
                IsDeleted = a.Skill.IsDeleted
            }).ToList();

            activityViewModel.Requirements = activity.ActivityRequirements.Select(a => new Requirement
            {
                Id = a.Requirement.Id,
                Name = a.Requirement.Name,
                IsDeleted = a.Requirement.IsDeleted
            }).ToList();

            return activityViewModel;
        }

        public async Task UpdateActivity(CreateActivityViewModel activityViewModel, int id)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.Id == id
                },
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
                                .Include(x => x.ActivityRequirements).ThenInclude(r => r.Requirement)
            };

            Activity activity = await _repository.GetAsync(dbContext, specification);

            activity.OrgId = activityViewModel.OrgId;
            activity.AreaId = activityViewModel.AreaId;
            activity.Name = activityViewModel.Name;
            activity.Address = activityViewModel.Address;
            activity.StartDate = activityViewModel.StartDate;
            activity.EndDate = activityViewModel.EndDate;
            activity.MemberQuantity = activityViewModel.MemberQuantity;
            activity.Description = activityViewModel.Description;
            activity.Mission = activityViewModel.Mission;
            activity.IsVirtual = activityViewModel.IsVirtual;
            activity.Website = activityViewModel.Website;
            activity.Banner = activityViewModel.Banner;
            activity.UpdatedBy = activityViewModel.OrgId;
            activity.UpdatedDate = DateTime.Now;

            activity.ActivitySkills = activityViewModel.Skills.Select(s => new ActivitySkill
            {
                SkillId = s.Id,
                ActivityId = activity.Id,
                IsDeleted = false
            }).ToList() ;

            activity.ActivityRequirements = activityViewModel.Requirements.Select(r => new ActivityRequirement
            {
                RequirementId = r.Id,
                ActivityId = activity.Id,
                IsDeleted = false
            }).ToList();

            await _repository.UpdateAsync(dbContext, activity);
        }

        public async Task DeleteActivity(int id)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            Activity activity = await _repository.GetByIdAsync<Activity>(dbContext, id);
            await _repository.DeleteAsync(dbContext, activity);
        }

        public async Task<ViewActivityViewModel> GetViewActivityViewModel(int id)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.Id == id
                },
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
                                .Include(x => x.ActivityRequirements).ThenInclude(r => r.Requirement)
                                .Include(x => x.Area)
            };

            Activity activity = await _repository.GetAsync(dbContext, specification);

            if (activity is null) return new ViewActivityViewModel();

            ViewActivityViewModel activityViewModel = new ViewActivityViewModel
            {
                OrgId = activity.OrgId,
                AreaId = activity.AreaId,
                Name = activity.Name,
                Address = activity.Address,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                MemberQuantity = activity.MemberQuantity,
                Description = activity.Description,
                Mission = activity.Mission,
                IsApproved = activity.IsApproved,
                IsVirtual = activity.IsVirtual,
                Website = activity.Website,
                Banner = activity.Banner,
                Area = activity.Area,
                ActivityImages = activity.ActivityImages
            };
            
            activityViewModel.Skills = activity.ActivitySkills.Select(a => new Skill
            {
                Id = a.SkillId,
                Name = a.Skill.Name,
                IsDeleted = a.Skill.IsDeleted
            }).ToList();

            activityViewModel.Requirements = activity.ActivityRequirements.Select(a => new Requirement
            {
                Id = a.Requirement.Id,
                Name = a.Requirement.Name,
                IsDeleted = a.Requirement.IsDeleted
            }).ToList();

            return activityViewModel;
        }
    }
}
