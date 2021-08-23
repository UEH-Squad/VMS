using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
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

        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            return await _repository.GetListAsync<Skill>(dbContext);
        }

        public async Task<List<Skill>> GetAllSubSkillsAsync(int parentSkillId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Skill> specification = new()
            {
                Conditions = new List<Expression<System.Func<Skill, bool>>>()
                {
                    s => s.ParentSkillId == parentSkillId
                }
            };

            return await _repository.GetListAsync(dbContext, specification);
        }
    }
}