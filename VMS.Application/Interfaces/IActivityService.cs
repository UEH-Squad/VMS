using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Services;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<PaginatedList<ActivityViewModel>> GetAllActivitiesAsync(bool isSearch, string searchValue, FilterActivityViewModel filter, int currentPage, Dictionary<ActOrderBy, bool> orderList = null, Coordinate userLocation = null);

        Task<List<ActivityViewModel>> GetFeaturedActivitiesAsync();

        Task<int> AddActivityAsync(CreateActivityViewModel activity);

        Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId);

        Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId);

        Task DeleteActivityAsync(int activityId);

        Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId);

        Task<List<UserWithActivityViewModel>> GetRelatedActivities(string userId, Coordinate location, bool isFeatured = false);

        Task<List<ActivityViewModel>> GetOrgActs(string id, StatusAct status);

        Task CloseOrDeleteActivity(int activityId, bool isClose = false, bool isDelete = false);

        Task UpdateActFavorAsync(int activityId, string userId);

        Task CloseActivityDailyAsync();
    }
}