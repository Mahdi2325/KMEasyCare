using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.Infrastructure
{
    public static class DateTimeExtension
    {
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return value.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }
    }
}
