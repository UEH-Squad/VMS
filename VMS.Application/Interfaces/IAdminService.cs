using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IAdminService
    {
        Task<bool> AddListAccountsAsync(List<CreateAccountViewModel> accounts, Role role);

        Task<bool> AddSingleAccountAsync(CreateAccountViewModel account, Role role);

        Task<PaginatedList<CreateAccountViewModel>> GetAllAccountsAsync(FilterAccountViewModel filter, int page);

        Task DeleteListAccountsAsync(List<string> listAccountIds);
    }
}
