using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IOrganizationService
    {
        Task<OrgViewModel> GetOrgAsync(string id);
        Task<List<OrgActViewModel>> GetOrgActs(string id);
        Task<List<OrgActViewModel>> GetOrgActFavourite(string Id);
        Task<List<OrgActViewModel>> GetOrgActCompleted(string Id);


    }
}
