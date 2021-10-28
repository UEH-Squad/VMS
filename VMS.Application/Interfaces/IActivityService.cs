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
        Task<PaginatedList<ActivityViewModel>> GetAllActivitiesAsync(FilterActivityViewModel filter, int currentPage, Dictionary<ActOrderBy, bool> orderList = null, Coordinate userLocation = null, int pageSize = 20);

        Task<PaginatedList<ActivityViewModel>> GetAllActivitiesAsync(Dictionary<ActOrderBy, bool> orderList = null, Coordinate userLocation = null, int pageSize = 8);

        Task<List<ActivityViewModel>> GetFeaturedActivitiesAsync();

        Task<int> AddActivityAsync(CreateActivityViewModel activity);

        Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId);

        Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId);

        Task DeleteActivityAsync(int activityId);

        Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId);

        Task<List<ActivityViewModel>> GetOrgActsAsync(string id, StatusAct status);

        Task CloseOrDeleteActivity(int activityId, bool isDelete = false, bool isClose = false);

        Task<List<ActivityViewModel>> GetAllUserActivityViewModelsAsync(string userId, StatusAct statusAct, DateTime dateTime);

        Task<PaginatedList<ActivityViewModel>> GetAllOrganizationActivityViewModelAsync(FilterOrgActivityViewModel filter, int currentPage);
    }
}