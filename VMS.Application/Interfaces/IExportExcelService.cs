using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Interfaces
{
    public interface IExportExcelService
    {
        byte[] ResultExportToExcel(PaginatedList<ListVolunteerViewModel> list, int actId);
    }
}
