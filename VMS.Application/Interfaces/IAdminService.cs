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

        Task<PaginatedList<AccountViewModel>> GetAllAccountsAsync(FilterAccountViewModel filter, int page, int pageSize = 20);

        Task<List<AccountViewModel>> GetAllAccountsByRoleAsync(Role role);

        Task DeleteListAccountsAsync(List<string> listAccountIds);

        Task<bool> UpdateAccountAsync(AccountViewModel account, Role role);
    }
}
