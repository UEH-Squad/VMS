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
    public class RequirementService : BaseService, IRequirementService
    {
        public RequirementService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<Requirement>> GetAllRequirements()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            return await _repository.GetListAsync<Requirement>(dbContext);
        }
    }
}
