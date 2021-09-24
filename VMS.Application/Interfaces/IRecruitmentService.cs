using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IRecruitmentService
    {
        Task<PaginatedList<RecruitmentViewModel>> GetAllRecruitmentsAsync(int activityId, int currentPage);
        Task UpdateRatingAsync(double rank, int? recruitmentId = null);
    }
}
