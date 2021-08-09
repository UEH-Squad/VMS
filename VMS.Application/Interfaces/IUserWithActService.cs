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
        Task<List<UserWithActivityViewModel>> GetFeaturedActivitiesWithoutUserLocAsync(double Lat, double Long);
        Task<List<UserWithActivityViewModel>> GetNewestActivitiesWithoutUserLocAsync(double Lat, double Long);
    }
}
