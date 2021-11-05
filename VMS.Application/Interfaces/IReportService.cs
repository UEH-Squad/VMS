using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IReportService
    {
        Task AddReportAsync(ReportViewModel reportViewModel);
    }
}