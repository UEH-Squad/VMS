using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class CategoryService : BaseService, ICategoryService
    {

        public CategoryService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory) : base(repository, dbContextFactory)
        {
        }

        public Task<List<Category>> GetAllCategories()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            return _repository.GetListAsync<Category>(context);
        }
    }
}