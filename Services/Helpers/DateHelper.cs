using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadAirInfo.Services.Helpers
{
    public class DateHelper
    {
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
