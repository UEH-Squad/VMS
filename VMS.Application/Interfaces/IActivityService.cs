using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<List<ActivityViewModel>> GetAllActivitiesAsync();

        Task AddActivityAsync(CreateActivityViewModel activity);

        Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId);

        Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId);

        Task DeleteActivityAsync(int activityId);

        Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId);

        Task<List<UserWithActivityViewModel>> GetRelatedActivities(string userId, Coordinate location, bool isFeatured = false);

        Task<List<OtherActivitiesViewModel>> GetOtherActivities(string orgId);
    }
}