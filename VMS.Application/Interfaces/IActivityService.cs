using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<List<ActivityViewModel>> GetAllActivities();
        Task AddActivity(CreateActivityViewModel activity);
        Task<CreateActivityViewModel> GetCreateActivityViewModel(int id);
        Task UpdateActivity(CreateActivityViewModel activityViewModel, int id);
        Task DeleteActivity(int id);
        Task<ViewActivityViewModel> GetViewActivityViewModel(int id);
    }
}
