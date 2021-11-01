using System;
using System.Collections.Generic;

namespace VMS.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static IEnumerable<DateTime> To(this DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        public static bool Between(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            return dateTime.Date >= startDate.Date && dateTime.Date <= endDate.Date;
        }

        public static IEnumerable<DateTime> GetDateRange(this DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException($"{nameof(endDate)} must be greater than or equal to {nameof(startDate)}");
            }

            while (startDate.Date <= endDate.Date)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}