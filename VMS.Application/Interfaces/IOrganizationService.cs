using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface IOrganizationService
    {
        User GetOrg(string orgId);
        UserViewModel GetOrgRating(string Id);
        Task UpdateUserAsync(UpdateUserViewModel userViewModel, string userId);
    }
}
