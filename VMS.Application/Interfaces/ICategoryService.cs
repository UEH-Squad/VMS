using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
    }
}
