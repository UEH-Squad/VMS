using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IExportExcelService
    {
        Task ResultExportToExcel(GenericRepository.PaginatedList<ListVolunteerViewModel> list);
    }
}
