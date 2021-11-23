using VMS.Common.Enums;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace VMS.Application.Interfaces
{
    public interface IExcelService
    {
        Task<bool> AddListAccountsFromExcelFileAsync(IBrowserFile file, Role role);

        byte[] ExportListAccountToExcel(List<AccountViewModel> accounts, Role role);
    }
}
