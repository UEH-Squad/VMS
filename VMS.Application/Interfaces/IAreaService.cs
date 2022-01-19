using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IAreaService
    {
        Task<List<AreaViewModel>> GetAllAreasAsync(bool isPinned = false);

        Task<PaginatedList<AreaViewModel>> GetAllAreasAsync(int pageIndex);

        Task UpdateListAreasAsync(List<AreaViewModel> areas);
    }
}