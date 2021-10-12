using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IFacultyService
    {
        Task<List<FacultyViewModel>> GetAllFacultiesAsync();
    }
}
