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
    public class SkillService : BaseService, ISkillService
    {
        public SkillService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<SkillViewModel>> GetAllSkillsAsync(int? parentSkillId = null)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            List<Skill> skills = await _repository.GetListAsync(dbContext, new Specification<Skill>()
            {
                Conditions = new List<Expression<Func<Skill, bool>>>()
                {
                    s => s.ParentSkillId == parentSkillId
                },
                Includes = s => s.Include(x => x.SubSkills)
            });

            return _mapper.Map<List<SkillViewModel>>(skills);
        }

        public async Task<PaginatedList<SkillViewModel>> GetAllSkillsAsync(int pageIndex)
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Skill> specification = new()
            {
                Conditions = new()
                {
                    x => !x.IsDeleted,
                    x => x.ParentSkillId == null
                },
                Includes = x => x.Include(x => x.SubSkills),
                PageSize = 20,
                PageIndex = pageIndex,
                OrderBy = x => x.OrderByDescending(x => x.Id)
            };

            var skills = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<PaginatedList<SkillViewModel>>(skills);
        }

        public async Task<List<SkillViewModel>> GetAllSkillsByNameAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new List<SkillViewModel>();
            }

            DbContext dbContext = _dbContextFactory.CreateDbContext();

            List<Skill> skills = await _repository.GetListAsync(dbContext, new Specification<Skill>()
            {
                Conditions = new List<Expression<Func<Skill, bool>>>()
                {
                    s => s.Name.ToLower().Contains(searchText.ToLower())
                }
            });

            return _mapper.Map<List<SkillViewModel>>(skills);
        }

        public async Task UpdateListSkillsAsync(List<SkillViewModel> skills)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Skill> specification = new()
            {
                Conditions = new()
                {
                    x => skills.Select(x => x.Id).Any(id => id == x.Id)
                },
                Includes = x => x.Include(x => x.SubSkills)
            };

            IEnumerable<Skill> listSkills = await _repository.GetListAsync(dbContext, specification);

            foreach (var skill in listSkills)
            {
                var skillViewModel = skills.Find(x => x.Id == skill.Id);

                skill.Name = skillViewModel.Name;
                skill.IsDeleted = skillViewModel.IsDeleted;

                if (skill.ParentSkill is null && skillViewModel.ParentSkillId is not null)
                {
                    skill.SubSkills.Clear();
                }

                skill.ParentSkillId = skillViewModel.ParentSkillId;
            }

            await _repository.UpdateAsync(dbContext, listSkills);
        }
    }
}