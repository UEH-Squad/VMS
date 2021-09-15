using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                          IIdentityService identityService,
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
            });

            if (org is null)
            {
                return null;
            }

            CreateOrgProfileViewModel orgProfileViewModel = _mapper.Map<CreateOrgProfileViewModel>(org);

            return orgProfileViewModel;
        }
    }
}
