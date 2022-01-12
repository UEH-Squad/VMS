using System.Threading.Tasks;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IReportService
    {
        Task AddReportAsync(ReportViewModel reportViewModel);

        Task<PaginatedList<ReportViewModel>> GetAllReportsAsync(FilterReportViewModel filter, int currentPage);

        Task UpdateReportStateAsync(int reportId, ReportState state, string handlerId);

        Task<ReportViewModel> GetReportByIdAsync(int reportId);
    }
}