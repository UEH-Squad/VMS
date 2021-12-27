using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IOrganizationService
    {
        UserViewModel GetOrgFull(string id);

        Task UpdateUserAsync(UpdateUserViewModel userViewModel, string userId);

        List<UserViewModel> GetAllOrganizers();

        Task<PaginatedList<UserViewModel>> GetAllOrganizers(FilterOrgViewModel filter, int currentPage);
    }
}
