using VMS.Common.Enums;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace VMS.Application.Interfaces
{
    public interface IExcelService
    {
        Task<bool> AddListAccountsFromExcelFileAsync(IBrowserFile file, Role role);
    }
}
