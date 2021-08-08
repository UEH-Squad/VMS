using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IUserWithActService
    {
        Task<List<UserWithActivityViewModel>> GetActivitiesAsync();
        Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithUserLocAsync();
        Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithUserLocAsync();
        Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithoutUserLocAsync();
        Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithoutUserLocAsync();
    }
}
