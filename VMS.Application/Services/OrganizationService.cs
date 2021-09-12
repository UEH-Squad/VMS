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
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
        public class OrganizationService : BaseService, IOrganizationService
        {
        public OrganizationService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<OrgViewModel> GetOrgAsync(string id)
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();
            Specification<User> GetOrgSpec = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<User, bool>>>
                {
                    a => a.Id == id
                },
                Includes = user => user.Include(x => x.UserAreas).ThenInclude(x=>x.Area).Include(x => x.Activities)
            };
            User Gets = await _repository.GetAsync(dbContext,GetOrgSpec);
            var GetOrg =  new OrgViewModel
            {
                UserAreas = Gets.UserAreas,
                ActivitiesNum = Gets.Activities.Count,
                CreateDate = Gets.CreatedDate.Year,
                Avatar = Gets.Avatar
            };
            return GetOrg;
        }

        public async Task<List<OrgActViewModel>> GetOrgActs(string Id)
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.OrgId == Id,
                    a=> a.EndDate >= DateTime.Now
                },
            };

            List<Activity> activity = await _repository.GetListAsync<Activity>(context, specification);

            IEnumerable<OrgActViewModel> orgActViewModels = activity.Select(x => new OrgActViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                ActivityBanner = x.Banner,
                Isclosed = x.IsClosed
            });

            return orgActViewModels.ToList();
        }

        public async Task<List<OrgActViewModel>> GetOrgActFavourite(string Id)
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.OrgId == Id,
                    a=> a.EndDate >= DateTime.Now
                },
                Includes = activities => activities.Include(x => x.Favorites)
            };

            List<Activity> activity = await _repository.GetListAsync<Activity>(context, specification);

            List<OrgActViewModel> orgActViewModels = activity.Select(x => new OrgActViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                ActivityBanner = x.Banner,
                Favorites = x.Favorites,
                Isclosed = x.IsClosed
            }).ToList();

            return orgActViewModels.OrderByDescending(a => a.Favorites.Count).Take(8).ToList();
        }

        public async Task<List<OrgActViewModel>> GetOrgActCompleted(string Id)
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Activity> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<Activity, bool>>>
                {
                    a => a.OrgId == Id,
                    a=> a.EndDate < DateTime.Now
                },
            };

            List<Activity> activity = await _repository.GetListAsync<Activity>(context, specification);

            List<OrgActViewModel> orgActViewModels = activity.Select(x => new OrgActViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                ActivityBanner = x.Banner,
                EndDate = x.EndDate
            }).ToList();
            return orgActViewModels.OrderByDescending(a => a.EndDate).ToList();
        }
    }
}
