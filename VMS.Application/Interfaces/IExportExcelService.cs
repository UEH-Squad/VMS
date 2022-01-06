using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IExportExcelService
    {
        byte[] ResultExportToExcel(List<ListVolunteerViewModel> list, int actId);
        Task<bool> UpdateListVolunteerFromExcelFileAsync(IBrowserFile file, int actId);
    }
}
