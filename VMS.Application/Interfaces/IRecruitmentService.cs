using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IRecruitmentService
    {
        Task<PaginatedList<RecruitmentViewModel>> GetAllRecruitmentsAsync(int activityId, int currentPage, string searchValue, bool? isRated);
        Task<PaginatedList<ListVolunteerViewModel>> GetListVolunteersAsync(int actId, string searchValue, bool isDeleted, int currentPage);
        Task UpdateVounteerAsync(List<int> list, bool isDeleted);
        Task UpdateRatingAndCommentAsync(int activityId, double? rank, string comment, int? recruitmentId = null);
        Task<List<ListVolunteerViewModel>> GetAllListVolunteerAsync(int actId);
        Task UpdateRecruitmentAsync(List<string> volunteers, int activityId);
    }
}
