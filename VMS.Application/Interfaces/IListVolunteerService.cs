using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Domain.Models;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IListVolunteerService
    {
        Task<PaginatedList<ListVolunteerViewModel>> GetListVolunteers(int actId, string searchValue, bool isDeleted, int currentPage);
        Task UpdateVounteer(int id, bool isDeleted);
    }
}
