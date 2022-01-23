using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface ISkillService
    {
        Task<List<SkillViewModel>> GetAllSkillsAsync(int? parentSkillId = null);

        Task<PaginatedList<SkillViewModel>> GetAllSkillsAsync(int pageIndex);

        Task<List<SkillViewModel>> GetAllSkillsByNameAsync(string searchText);

        Task UpdateListSkillsAsync(List<SkillViewModel> skills);

        Task AddSkillAsync(SkillViewModel skillViewModel);
    }
}
