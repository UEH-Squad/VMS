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
    public class OrgService : BaseService, IOrgService
    {
        public OrgService(IRepository repository,
                          IDbContextFactory<VmsDbContext> dbContextFactory,
                          IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<CreateOrgProfileViewModel> GetCreateOrgProfileViewModelAsync(string orgId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User org = await _repository.GetAsync(dbContext, new Specification<User>()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == orgId
                },
                Includes = a => a.Include(x => x.UserAreas).ThenInclude(s => s.Area)
            });

            if (org is null)
            {
                return null;
            }

            CreateOrgProfileViewModel orgProfileViewModel = _mapper.Map<CreateOrgProfileViewModel>(org);

            orgProfileViewModel.Areas = org.UserAreas.Select(a => new AreaViewModel
            {
                Id = a.AreaId,
                Name = a.Area.Name,
                Icon = a.Area.Icon
            }).ToList();

            return orgProfileViewModel;
        }

        public async Task UpdateOrgProfile(CreateOrgProfileViewModel orgProfileViewModel, string orgId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User org = await _repository.GetAsync(dbContext, new Specification<User>()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == orgId
                },
                Includes = a => a.Include(x => x.UserAreas).ThenInclude(s => s.Area)
            });

            org = _mapper.Map(orgProfileViewModel, org);

            org.UserAreas = MapAreas(orgProfileViewModel, org);

            await _repository.UpdateAsync(dbContext, org);
        }

        private static ICollection<UserArea> MapAreas(CreateOrgProfileViewModel orgProfileViewModel, User org)
        {
            return orgProfileViewModel.Areas.Select(s => new UserArea
            {
                User = org,
                AreaId = s.Id
            }).ToList();
        }
    }
}
