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
    public class InfoActivityService : BaseService, IInfoActivityService
    {
        public InfoActivityService(IIdentityService identityService, 
                                   IRepository repository, 
                                   IDbContextFactory<VmsDbContext> dbContextFactory, 
                                   IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }
        public async Task<List<InfoActivityViewModel>> GetInfoActivity()
        {
            DbContext context = _dbContextFactory.CreateDbContext();

            Specification<Activity> specification = new()
            {
                Includes = a => a.Include(x => x.ActivitySkills).ThenInclude(s => s.Skill)
            };

            List<Activity> activity = await _repository.GetListAsync<Activity>(context, specification);

            IEnumerable<InfoActivityViewModel> infoActivity = activity.Select(x => new InfoActivityViewModel
            {
                ActivityId = x.Id,
                ActivityName = x.Name,
                ActivityPostDate = x.PostDate,
                ActivityStartDate = x.StartDate,
                ActivityEndDate = x.EndDate,
                ActivityDescription = x.Description,
                ActivityMission = x.Mission,
                ActivityCommission = x.Commission,
                ActivityRequirement = x.Requirement,
                ActivityAddress = x.Address,
                ActivitySkill = x.ActivitySkills.Select(a => new Skill
                {
                    Name = a.Skill.Name,
                }).ToList(),
                ActivityOrgId = x.OrgId,
            });

            return infoActivity.ToList();
        }
    }
}