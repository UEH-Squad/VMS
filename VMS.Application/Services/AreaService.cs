using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
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

        public async Task<List<AreaViewModel>> GetAllAreasAsync()
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();
            List<Area> areas = await _repository.GetListAsync<Area>(dbContext);
            return _mapper.Map<List<AreaViewModel>>(areas);
        }
    }
}