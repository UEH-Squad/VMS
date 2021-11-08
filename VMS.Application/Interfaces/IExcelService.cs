using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace VMS.Application.Interfaces
{
    public interface IExcelService
    {
        Task<List<CreateAccountViewModel>> GetListAccountFromExcelFileAsync(IBrowserFile file);
    }
}
