using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IHolidaysApiService
    {
        Task<IEnumerable<HolidayResponseModel>> GetHolidays(HolidayRequestModel holidaysRequest);
    }
}