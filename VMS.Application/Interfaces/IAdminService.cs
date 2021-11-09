using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common.Enums;

namespace VMS.Application.Interfaces
{
    public interface IAdminService
    {
        Task AddListUserAsync(List<CreateAccountViewModel> accounts, Role role);
    }
}
