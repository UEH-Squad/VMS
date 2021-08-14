using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<List<ActivityViewModel>> GetAllActivitiesAsync(FilterActivityViewModel filter);
        Task AddActivityAsync(CreateActivityViewModel activity);
        Task<CreateActivityViewModel> GetCreateActivityViewModelAsync(int activityId);
        Task UpdateActivityAsync(CreateActivityViewModel activityViewModel, int activityId);
        Task DeleteActivityAsync(int activityId);
        Task<ViewActivityViewModel> GetViewActivityViewModelAsync(int activityId);
    }
}
