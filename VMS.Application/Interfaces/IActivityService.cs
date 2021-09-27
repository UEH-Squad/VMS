using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Services;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<PagedResult<ActivityViewModel>> GetAllActivitiesAsync(bool isSearch, string searchValue, FilterActivityViewModel filter, Dictionary<ActOrderBy, bool> orderList, Coordinate userLocation, int currentPage);

        Task<List<ActivityViewModel>> GetFeaturedActivitiesAsync();

        Task<int> AddActivityAsync(CreateActivityViewModel activity);

        Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId);

        Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId);

        Task DeleteActivityAsync(int activityId);

        Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId);

        Task<List<UserWithActivityViewModel>> GetRelatedActivities(string userId, Coordinate location, bool isFeatured = false);
        Task<List<ActivityViewModel>> GetOrgActs(string Id, StatusAct status);
        Task UpdateStatusActAsync(int activityId, bool close, bool delete);
    }
}