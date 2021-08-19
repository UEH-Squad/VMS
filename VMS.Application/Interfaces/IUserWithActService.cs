using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IUserWithActService
    {
        Task<List<UserWithActivityViewModel>> GetRelatedActivities(string userId, UserLocation location, bool isFeatured = false);
    }
}