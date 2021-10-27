using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Services;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<PaginatedList<ActivityViewModel>> GetAllActivitiesAsync(bool isSearch, string searchValue, FilterActivityViewModel filter, int currentPage, Dictionary<ActOrderBy, bool> orderList = null, Coordinate userLocation = null, int pageSize = 20);

        Task<PaginatedList<ActivityViewModel>> GetAllActivitiesAsync(Dictionary<ActOrderBy, bool> orderList = null, Coordinate userLocation = null, int pageSize = 8);

        Task<List<ActivityViewModel>> GetFeaturedActivitiesAsync();

        Task<int> AddActivityAsync(CreateActivityViewModel activity);

        Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId);

        Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId);

        Task DeleteActivityAsync(int activityId);

        Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId);

        Task<List<ActivityViewModel>> GetOrgActs(string id, StatusAct status);

        Task UpdateStatusActAsync(int activityId, bool close, bool delete);

        Task CloseOrDeleteActivity(int activityId, bool isDelete = false, bool isClose = false);

        Task UpdateActFavorAsync(int activityId, string userId);

        Task<List<ActivityViewModel>> GetAllUserActivityViewModelsAsync(string userId, StatusAct statusAct, DateTime dateTime = new());

        Task<PaginatedList<ActivityViewModel>> GetAllOrganizationActivityViewModelAsync(FilterOrgActivityViewModel filter, int currentPage);
    }
}