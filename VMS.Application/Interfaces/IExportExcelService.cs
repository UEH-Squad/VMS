using System.Collections.Generic;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IExportExcelService
    {
        byte[] ResultExportToExcel(List<ListVolunteerViewModel> list, int actId);
    }
}
