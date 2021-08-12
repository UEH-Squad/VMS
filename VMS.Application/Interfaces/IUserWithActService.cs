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
        Task<List<UserWithActivityViewModel>> GetActivitiesWithUserLocAsync(double Lat, double Long);
        Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithUserLocAsync(double Lat, double Long);
        Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithUserLocAsync(double Lat, double Long);
        Task<List<UserWithActivityViewModel>> GetActivitiesWithoutUserLocAsync();
        Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithoutUserLocAsync();
        Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithoutUserLocAsync();
        Task<List<UserWithActivityViewModel>> GetActivitiesWithDistristAsync();
        Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithDistristAsync();
        Task<List<UserWithActivityViewModel>> GetfeaturedActivitiesWithDistristAsync();
    }
}
