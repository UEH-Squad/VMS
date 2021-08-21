using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AreaService : BaseService, IAreaService
    {
        public AreaService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<Area>> GetAllAreasAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            return await _repository.GetListAsync<Area>(dbContext);
        }
    }
}
