using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class AreaService : BaseService, IAreaService
    {
        public AreaService(IRepository repository,
                           IDbContextFactory<VmsDbContext> dbContextFactory,
                           IMapper mapper)
            : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<AreaViewModel>> GetAllAreasAsync(bool isPinned = false)
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();

            List<Area> areas = await _repository.GetListAsync<Area>(dbContext);

            if (isPinned)
            {
                return _mapper.Map<List<AreaViewModel>>(areas.Where(x => x.IsPinned).Take(4));
            }
            else
            {
                return _mapper.Map<List<AreaViewModel>>(areas.OrderByDescending(x => x.IsPinned));
            }
        }

        public async Task<PaginatedList<AreaViewModel>> GetAllAreasAsync(int pageIndex)
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Area> specification = new()
            {
                PageIndex = pageIndex,
                PageSize = 20,
                OrderBy = x => x.OrderByDescending(x => x.IsPinned)
                                .ThenByDescending(x => x.Id)
            };

            var areas = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<PaginatedList<AreaViewModel>>(areas);
        }

        public async Task UpdateListAreasAsync(List<AreaViewModel> areas)
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Area> specification = new()
            {
                Conditions = new()
                {
                    x => areas.Select(x => x.Id).Any(id => id == x.Id)
                }
            };

            var listAreas = await _repository.GetListAsync(dbContext, specification);

            foreach (Area area in listAreas)
            {
                var areaViewModel = areas.Find(x => x.Id == area.Id);
                area.IsPinned = areaViewModel.IsPinned;
                area.IsDeleted = areaViewModel.IsDeleted;
                area.Name = areaViewModel.Name;
                area.Color = areaViewModel.Color;
                area.Icon = areaViewModel.Icon;
            }

            //listAreas = _mapper.Map(areas, listAreas);

            await _repository.UpdateAsync<Area>(dbContext, listAreas);
        }
    }
}