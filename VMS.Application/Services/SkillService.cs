using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class SkillService : BaseService, ISkillService
    {
        public SkillService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<SkillViewModel>> GetAllSkillsAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Skill> specification = new()
            {
                Includes = s => s.Include(x => x.SubSkills)
            };

            List<Skill> skills = await _repository.GetListAsync<Skill>(dbContext);

            return _mapper.Map<List<SkillViewModel>>(skills);
        }

        public async Task<List<SkillViewModel>> GetAllSubSkillsAsync(int parentSkillId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Skill> specification = new()
            {
                Conditions = new List<Expression<System.Func<Skill, bool>>>()
                {
                    s => s.ParentSkillId == parentSkillId
                }
            };

            List<Skill> skills = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<List<SkillViewModel>>(skills);
        }
    }
}