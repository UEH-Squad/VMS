using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VMS.Domain.Interfaces;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class BaseService
    {
        protected readonly IRepository _repository;
        protected readonly IDbContextFactory<VmsDbContext> _dbContextFactory;

        public BaseService(IRepository repository,
                           IDbContextFactory<VmsDbContext> dbContextFactory)
        {
            _repository = repository;
            _dbContextFactory = dbContextFactory;
        }
    }
}