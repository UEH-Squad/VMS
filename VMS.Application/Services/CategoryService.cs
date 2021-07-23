using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;

namespace VMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository _repository;

        public CategoryService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Category>> GetAllCategories()
        {
            return _repository.GetListAsync<Category>();
        }
    }
}