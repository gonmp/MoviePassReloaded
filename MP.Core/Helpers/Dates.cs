using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Helpers
{
    public static class Dates
    {
        public static DateTime GetNextDayOfWeek(int dayOfWeek)
        {
            var today = DateTime.Now;

            var daysUntilDayOfWeek = (dayOfWeek - (int)today.DayOfWeek);

            if (daysUntilDayOfWeek < 0)
            {
                daysUntilDayOfWeek += 7;
            }

            var nextDayOfWeek = today.AddDays(daysUntilDayOfWeek);

            return nextDayOfWeek;
        }
    }
}
