using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface IAdminService
    {
        Task AddListUsersAsync(List<CreateAccountViewModel> accounts, Role role);
    }
}
