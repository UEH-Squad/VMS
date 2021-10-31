using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IRecruitmentService
    {
        Task<PaginatedList<ListVolunteerViewModel>> GetListVolunteersAsync(int actId, string searchValue, bool isDeleted, int currentPage);
        Task UpdateVounteerAsync(List<int> list, bool isDeleted);
    }
}
