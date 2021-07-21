using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Holidays
{
    public partial class Holidays
    {
        private bool isFirstLoad = true;
        private readonly HolidayRequestModel holidayModel = new();
        private IEnumerable<HolidayResponseModel> holidays;

        [Inject]
        protected IHolidaysApiService HolidaysApiService { get; set; }

        private async Task HandleValidSubmit()
        {
            isFirstLoad = false;
            holidays = null;
            holidays = await HolidaysApiService.GetHolidays(holidayModel);
        }
    }
}